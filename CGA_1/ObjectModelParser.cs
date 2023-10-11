using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
namespace CGA_1
{
    class ObjectModelParser
    {
        const short VALUES_NUM = 4;

        public EventHandler OnObjectParsed;

        public void ParseObjFile(ref ObjModel model, in string filePath)
        {
            model = new ObjModel();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = String.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');

                    if (parts.Length > 0)
                    {
                        // Вершины
                        if (parts[0] == "v" && parts.Length == VALUES_NUM)
                        {
                            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            
                            model.Vertices.Add(new Vector3(x, y, z));
                        }

                        // Полигоны -> ребра
                        else if (parts[0] == "f" && parts.Length == VALUES_NUM)
                        {
                            int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                            int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                            int v3 = int.Parse(parts[3].Split('/')[0]) - 1;

                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v1), model.Vertices.ElementAt(v2)));
                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v2), model.Vertices.ElementAt(v3)));
                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v3), model.Vertices.ElementAt(v1)));
                            /*model.Edges.Add(new Tuple<int, int>(v1, v2));
                            model.Edges.Add(new Tuple<int, int>(v2, v3));
                            model.Edges.Add(new Tuple<int, int>(v3, v1));*/
                        }
                        else if (parts[0] == "f" && parts.Length == (VALUES_NUM+1))
                        {
                            int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                            int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                            int v3 = int.Parse(parts[3].Split('/')[0]) - 1;
                            int v4 = int.Parse(parts[4].Split('/')[0]) - 1;

                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v1), model.Vertices.ElementAt(v2)));
                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v2), model.Vertices.ElementAt(v3)));
                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v3), model.Vertices.ElementAt(v4)));
                            model.Edges.Add(new Tuple<Vector3, Vector3>(model.Vertices.ElementAt(v4), model.Vertices.ElementAt(v1)));
                            /*model.Edges.Add(new Tuple<int, int>(v1, v2));
                            model.Edges.Add(new Tuple<int, int>(v2, v3));
                            model.Edges.Add(new Tuple<int, int>(v3, v4));
                            model.Edges.Add(new Tuple<int, int>(v4, v1));*/
                        }
                    }
                }
            }

            OnObjectParsed?.Invoke(this, EventArgs.Empty);
        }
    }
}
