using Split.Model;
using Split.Util;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Split.ViewModel
{
    public class VmDelete: BaseInotifyPropertyChanged
    {

        private Layout _layout_tmp;
        public Layout Layout_Tmp
        {
            get { return this._layout_tmp; }
        }

        private Field _field_tmp;
        public Field Field_Tmp
        {
            get { return this._field_tmp; }
        }

        private ObservableCollection<Layout> layout;
        private int indexLayout;
        private int idLayout;

        private View.DeleteLayout vDeleteLayout;

        private View.DeleteField vDeleteField;
        private int indexField;

        public Command Close { get; set; }
        public Command DeleteField { get; set; }
        public Command DeleteLayout { get; set; }

        /// <summary>
        /// Field
        /// </summary>
        /// <param name="layouts"></param>
        /// <param name="id"></param>
        /// <param name="viewDelet"></param>
        public VmDelete(ObservableCollection<Layout> layouts, string id, View.DeleteField viewDelete)
        {
            vDeleteField = viewDelete;
            layout = layouts;

            try
            {
                idLayout = Regexp.GetIdLayout(id);

                indexLayout = Query.GetIndexLayout(layout, (int)idLayout);

                indexField = Query.GetIndexField(idLayout, id);

                _layout_tmp = Query.GetLayout($"{idLayout}");

                _field_tmp = Query.GetField(idLayout, indexField);

                Close = new Command(this.close, () => { return true; });
                DeleteField = new Command(this.deleteField, () => { return true; });

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Layout
        /// </summary>
        /// <param name="layouts"></param>
        /// <param name="id"></param>
        /// <param name="viewDelet"></param>
        public VmDelete(ObservableCollection<Layout> layouts, string id, View.DeleteLayout viewDelete)
        {
            vDeleteLayout = viewDelete;
            layout = layouts;

            try
            {
                idLayout = Regexp.GetIdLayout(id);

                indexLayout = Query.GetIndexLayout(layout, (int)idLayout);

                _layout_tmp = Query.GetLayout($"{idLayout}");

                Close = new Command(this.close, () => { return true; });
                DeleteLayout = new Command(this.deleteLayout, () => { return true; });

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private void deleteLayout()
        {
            string nameLayout = Layout_Tmp.Name;

            layout.RemoveAt(indexLayout);

            FileInfo pathLayout = new FileInfo(Query.GetLayoutName(idLayout));

            var rmSucess = IO.removeDirectory(pathLayout.Directory.FullName);


            if (rmSucess)
                vDeleteLayout.msgDeleteLayout($"{idLayout}", nameLayout, "Excluído");
            else
                throw new Exception("Não foi possível excluir layout");

        }

        private void deleteField()
        {
            string nameLayout = Layout_Tmp.Name;

            Layout layout_aux = Query.GetLayout($"{idLayout}");

            layout_aux.Fields.RemoveAt(indexField);

            layout.RemoveAt(indexLayout);

            layout.Insert(indexLayout, layout_aux);

            var fullNameLayout = Query.GetLayoutName(idLayout);

            Json.SaveObject(fullNameLayout, layout[indexLayout]);

            vDeleteField.msgDeleteField(_field_tmp, nameLayout, "Excluído");

        }

        private void close()
        {
            if (vDeleteField != null)
                vDeleteField.Close();
            else
                if (vDeleteLayout != null)
                vDeleteLayout.Close();
        }
    }
}
