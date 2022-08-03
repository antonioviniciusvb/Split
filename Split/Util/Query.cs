using System.Linq;
using System.IO;
using Split.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Split.Util
{
    public static class Query
    {
        private static DirectoryInfo directoryInfo = new DirectoryInfo(Util.Global_Variable.PathLayout);

        public static int LastIdLayout()
        {

            if (directoryInfo.GetDirectories("*.*").Count() == 0)
            {
                return 0;
            }
            else
            {
                var lastId = (from dir in directoryInfo.GetDirectories("*.*")
                              orderby dir.CreationTime
                              select dir.Name).Last();

                return int.Parse(lastId);
            }
        }


        public static Field GetField(int idLayout, int indexField)
        {
            var file = Directory.GetFiles($"{directoryInfo.FullName}\\{idLayout}\\", "*.lyt*")[0];

            var layout = Util.Json.FileJsonToLayout(file);

            return layout.Fields[indexField];
        }

        public static int GetIndexField(int idLayout, string idField)
        {
            var file = GetFileLayout(idLayout);

            var layout = Util.Json.FileJsonToLayout(file);

            var index = layout.Fields.ToList<Field>().FindIndex(x => x.Id == $"{idField}");

            return index;
        }

        public static int GetNextIdField(int idLayout)
        {

            var file = GetFileLayout(idLayout);

            var layout = Util.Json.FileJsonToLayout(file);

            if(layout.Fields.Count == 0)
                return 1;

            var next_Id = layout.Fields.ToList<Field>()
                  .Where(x => x.Id != "")
                  .Select(x => new
                  {
                      Id = Regexp.GetIdField(x.Id)
                  })
                  .OrderByDescending(x => x.Id).First();

            return next_Id.Id;
        }


        #region Layout

        public static List<FileInfo> GetLayouts()
        {
            return directoryInfo.GetFiles("*.lyt*", System.IO.SearchOption.AllDirectories).OrderBy(f => f.CreationTime).ToList<FileInfo>();
        }

        public static int GetIndexLayout(ObservableCollection<Layout> layouts, int idLayout)
        {
            var index = layouts.ToList<Layout>().FindIndex(x => x.Id == idLayout);

            return index;
        }

        public static Layout GetLayout(string id)
        {
            var file = Directory.GetFiles($"{directoryInfo.FullName}\\{id}\\", "*.lyt*")[0];

            var layout = Util.Json.FileJsonToLayout(file);

            if (id == $"{layout.Id}")
                return layout;

            throw new System.Exception("Layout não localizado");

        }

        public static string GetLayoutName(int id)
        {
            var file = Directory.GetFiles($"{directoryInfo.FullName}\\{id}\\", "*.lyt*")[0];

            return file;
        }

        public static string GetFileLayout(int idLayout)
        {
            return Directory.GetFiles($"{directoryInfo.FullName}\\{idLayout}\\", "*.lyt*")[0];
        }

        public static string NextDirLayout()
        {

            if (directoryInfo.GetDirectories("*.*").Count() == 0)
            {
                return $"{Util.Global_Variable.PathLayout}\\0";
            }
            else
            {
                var lastId = (from dir in directoryInfo.GetDirectories("*.*")
                              orderby dir.CreationTime
                              select dir.Name).Last();

                return $"{Util.Global_Variable.PathLayout}\\{int.Parse(lastId)+1}";
            }
        }

        public static int NextIdLayout()
        {

            if (directoryInfo.GetDirectories("*.*").Count() == 0)
            {
                return 0;
            }
            else
            {
                var lastId = (from dir in directoryInfo.GetDirectories("*.*")
                              orderby dir.CreationTime
                              select dir.Name).Last();

                return int.Parse(lastId) + 1;
            }
        }

        public static string LastDirLayout()
        {
            if (directoryInfo.GetDirectories("*.*").Count() == 0)
            {
                return $"{Util.Global_Variable.PathLayout}\\0";
            }
            else
            {
                var lastId = (from dir in directoryInfo.GetDirectories("*.*")
                              orderby dir.CreationTime
                              select dir.Name).Last();

                return $"{Util.Global_Variable.PathLayout}\\{int.Parse(lastId)}";
            }
        }

        #endregion


    }
}
