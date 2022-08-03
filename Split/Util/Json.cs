using Newtonsoft.Json;
using Split.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Split.Util
{
    public static class Json
    {
        public static void SaveObject(string output, Layout layout)
        {
            //open file stream
            using (StreamWriter file = System.IO.File.CreateText(output))
            {
                JsonSerializer serializer = new JsonSerializer();

                var json = JsonConvert.SerializeObject(layout);

                var json_Hex = IO.String_To_Hex(json);

                file.WriteLine(json_Hex);

            }
        }

        public static Layout FileJsonToLayout(string path)
        {
            Layout layout = JsonConvert.DeserializeObject<Layout>
            (IO.Hex_To_Ansi(System.IO.File.ReadAllText(path)));

            return layout;
        }

        public static ObservableCollection<Layout> FileJsonToLayout(List<FileInfo> files)
        {
            ObservableCollection<Layout> layouts = new ObservableCollection<Layout>();

            for (int i = 0; i < files.Count; i++)
            {
                Layout layout = JsonConvert.DeserializeObject<Layout>
                (IO.Hex_To_Ansi(System.IO.File.ReadAllText(files[i].FullName)));

                layouts.Add(layout);
            }
            return layouts;
        }
    }
}
