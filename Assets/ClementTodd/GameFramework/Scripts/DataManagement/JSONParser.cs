using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd.DataManagement
{
    public static class JSONParser
    {
        private enum ReadContext
        {
            None,
            FindNextNode,
            PopulateList,
            PopulateDictionary,
            ReadKey,
            ReadString,
            ReadNumber,
            ReadKeyword
        }

        private class Node
        {
            public string key;
            public object value;

            public Node parent;
            public List<Node> children = new List<Node>();

            public Node(string key, object value, Node parent)
            {
                this.key = key;
                this.value = value;
                this.parent = parent;

                if (parent != null)
                {
                    parent.children.Add(this);
                }
            }
        }

        private class PendingKey
        {
            private string key = string.Empty;

            public void Add(char c)
            {
                key += c;
            }

            public string Get()
            {
                string finalKey = key;
                key = string.Empty;
                return finalKey;
            }
        }

        public static object Read(string json)
        {
            Node currentNode = null;
            PendingKey key = new PendingKey();

            Stack<ReadContext> context = new Stack<ReadContext>();
            context.Push(ReadContext.FindNextNode);

            for (int i = 0; i < json.Length; i++)
            {
                ReadContext currentContext = context.Peek();
                ReadContext contextToPush = ReadContext.None;
                bool popCurrentContext = false;
                bool endCurrentNode = false;
                bool repeatCurrentChar = false;

                switch (currentContext)
                {
                    case ReadContext.FindNextNode:
                        {
                            if (json[i] == '[')
                            {
                                currentNode = new Node(key.Get(), new List<object>(), currentNode);
                                contextToPush = ReadContext.PopulateList;

                                popCurrentContext = true;
                            }
                            else if (json[i] == '{')
                            {
                                currentNode = new Node(key.Get(), new Dictionary<string, object>(), currentNode);
                                contextToPush = ReadContext.PopulateDictionary;

                                popCurrentContext = true;
                            }
                            else if (json[i] == '"')
                            {
                                currentNode = new Node(key.Get(), string.Empty, currentNode);
                                contextToPush = ReadContext.ReadString;

                                popCurrentContext = true;
                            }
                            else if (char.IsDigit(json[i]))
                            {
                                currentNode = new Node(key.Get(), string.Empty, currentNode);
                                contextToPush = ReadContext.ReadNumber;

                                popCurrentContext = true;
                                repeatCurrentChar = true;
                            }
                            else if (char.IsLetter(json[i]))
                            {
                                currentNode = new Node(key.Get(), string.Empty, currentNode);
                                contextToPush = ReadContext.ReadKeyword;

                                popCurrentContext = true;
                                repeatCurrentChar = true;
                            }
                        }
                        break;

                    case ReadContext.PopulateList:
                        {
                            if (json[i] == ']')
                            {
                                endCurrentNode = true;
                            }
                            else if (json[i] == '[' || json[i] == '{' || json[i] == '"' || char.IsLetterOrDigit(json[i]))
                            {
                                contextToPush = ReadContext.FindNextNode;
                                repeatCurrentChar = true;
                            }
                        }
                        break;

                    case ReadContext.PopulateDictionary:
                        {
                            if (json[i] == '"')
                            {
                                contextToPush = ReadContext.ReadKey;
                            }
                            else if (json[i] == '}')
                            {
                                endCurrentNode = true;
                            }
                        }
                        break;

                    case ReadContext.ReadKey:
                        {
                            if (json[i] == '\\' && i + 1 < json.Length)
                            {
                                i += 1;
                                char escapeCharacer;
                                if (TryGetEscapeCharacter(json[i], out escapeCharacer))
                                {
                                    key.Add(escapeCharacer);
                                }
                            }
                            else if (json[i] == '"')
                            {
                                popCurrentContext = true;
                                contextToPush = ReadContext.FindNextNode;
                            }
                            else
                            {
                                key.Add(json[i]);
                            }
                        }
                        break;

                    case ReadContext.ReadString:
                        {
                            string nodeString = (string)currentNode.value;

                            if (json[i] == '\\' && i + 1 < json.Length)
                            {
                                i += 1;
                                char escapeCharacer;
                                if (TryGetEscapeCharacter(json[i], out escapeCharacer))
                                {
                                    nodeString += escapeCharacer;
                                    currentNode.value = nodeString;
                                }
                            }
                            else if (json[i] == '"')
                            {
                                endCurrentNode = true;
                            }
                            else
                            {
                                nodeString += json[i];
                                currentNode.value = nodeString;
                            }
                        }
                        break;

                    case ReadContext.ReadNumber:
                        {
                            string nodeString = (string)currentNode.value;

                            if (char.IsDigit(json[i]) || json[i] == '.')
                            {
                                nodeString += json[i];
                                currentNode.value = nodeString;
                            }
                            else
                            {
                                endCurrentNode = true;
                                repeatCurrentChar = true;

                                if (nodeString[nodeString.Length - 1] == '.')
                                {
                                    nodeString += '0';
                                }

                                int decimalCount = nodeString.Split('.').Length - 1;

                                if (decimalCount == 0)
                                {
                                    currentNode.value = int.Parse(nodeString);
                                }
                                else if (decimalCount == 1)
                                {
                                    currentNode.value = float.Parse(nodeString);
                                }
                                else
                                {
                                    Debug.LogWarningFormat("Invalid JSON number '{0}'. Value will be used as a string instead.", nodeString);
                                    currentNode.value = nodeString;
                                }
                            }
                        }
                        break;

                    case ReadContext.ReadKeyword:
                        {
                            string nodeString = (string)currentNode.value;

                            if (char.IsLetter(json[i]))
                            {
                                nodeString += json[i];
                                currentNode.value = nodeString;
                            }
                            else
                            {
                                endCurrentNode = true;
                                repeatCurrentChar = true;

                                switch (nodeString)
                                {
                                    case "true":
                                        currentNode.value = true;
                                        break;

                                    case "false":
                                        currentNode.value = false;
                                        break;

                                    case "null":
                                        currentNode.value = null;
                                        break;

                                    default:
                                        Debug.LogWarningFormat("Invalid JSON keyword '{0}'. Value will be used as a string instead.", nodeString);
                                        break;
                                }
                            }
                        }
                        break;
                }

                if (endCurrentNode || popCurrentContext)
                {
                    context.Pop();

                    if (endCurrentNode)
                    {
                        if (context.Count > 0)
                        {
                            switch (context.Peek())
                            {
                                case ReadContext.PopulateList:
                                    List<object> list = (List<object>)currentNode.parent.value;
                                    list.Add(currentNode.value);
                                    break;

                                case ReadContext.PopulateDictionary:
                                    Dictionary<string, object> dictionary = (Dictionary<string, object>)currentNode.parent.value;
                                    dictionary.Add(currentNode.key, currentNode.value);
                                    break;
                            }
                        }

                        if (currentNode.parent != null)
                        {
                            currentNode = currentNode.parent;
                        }
                        else
                        {
                            return currentNode.value;
                        }
                    }
                }

                if (contextToPush != ReadContext.None)
                {
                    context.Push(contextToPush);
                }

                if (repeatCurrentChar)
                {
                    i--;
                }
            }

            Debug.LogError("JSON string ended unexpectedly.");
            return null;
        }

        public static T Read<T>(string json)
        {
            object data = Read(json);

            if (data.GetType() != typeof(T))
            {
                Debug.LogError("Root node of parsed JSON was not of the requested type!");
                return default;
            }

            return (T)data;
        }

        public static List<object> ReadArray(string jsonObject)
        {
            return Read<List<object>>(jsonObject);
        }

        public static Dictionary<string, object> ReadObject(string jsonObject)
        {
            return Read<Dictionary<string, object>>(jsonObject);
        }

        public static string Write(object data)
        {
            string json = null;

            return json;
        }

        private static bool TryGetEscapeCharacter(char input, out char escapeCharacter)
        {
            switch (input)
            {
                case 'b':
                    escapeCharacter = '\b';
                    return true;

                case 'f':
                    escapeCharacter = '\f';
                    return true;

                case 'n':
                    escapeCharacter = '\n';
                    return true;

                case 'r':
                    escapeCharacter = '\r';
                    return true;

                case 't':
                    escapeCharacter = '\t';
                    return true;

                case '"':
                    escapeCharacter = '\"';
                    return true;

                case '\\':
                    escapeCharacter = '\\';
                    return true;

                default:
                    escapeCharacter = ' ';
                    return false;
            }
        }
    }
}