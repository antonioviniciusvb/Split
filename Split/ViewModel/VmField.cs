using System;
using System.Text;
using Split.Model;
using System.Windows;
using System.Windows.Forms;
using Split.Util;
using System.Collections.ObjectModel;

namespace Split.ViewModel
{
    public class VmField:BaseInotifyPropertyChanged
    {
        #region Config

        private Counter _config_counter;
        public Counter ConfigCounter
        {
            get { return _config_counter; }
        }

        private Date _config_date;
        public Date ConfigDate
        {
            get { return _config_date; }
        }

        private Time _config_time;
        public Time ConfigTime
        {
            get { return _config_time; }
        }

        private File _config_file;
        public File Config_File
        {
            get { return _config_file; }
        }


        private Test _config_Test;
        public Test Confi_Test
        {
            get { return _config_Test; }
        }


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

        #region Clear
        private void clearConfigCounter()
        {
            ConfigCounter.Clear();
        }

        private void clearConfigTime()
        {
            ConfigTime.Clear();
        }

        private void clearConfigDate()
        {
            ConfigDate.Clear();
        }


        private void clearConfigFile()
        {
            Config_File.Clear();
        }
        #endregion

        #endregion

        #region Commands
        public Command ClearConfigDate { get; set; }
        public Command ClearConfigTime { get; set; }
        public Command DefineFont { get; set; }
        public Command DefineForeground { get; set; }
        public Command DefineTestPage { get; set; }
        public Command ClearConfigCounter { get; set; }
        public Command ClearConfigFile { get; set; }
        public Command AddDate { get; set; }
        public Command AddTime { get; set; }
        public Command AddCounter { get; set; }
        public Command AddFile { get; set; }
        public Command TestField { get; set; }
        public Command SaveField { get; set; }


        #endregion

        #region Panels
        private int _indexType;
        public int Index_Type
        {
            get { return _indexType; }
            set
            {
                this._indexType = value;
                OnPropertyChanged("Index_Type");

                LogicVisibility = value == 0 ? Visibility.Visible : Visibility.Collapsed;
                Index_Logic = value == 0 ? Index_Logic : 2;
                setPanelsVrLogics();

            }
        }

        private Visibility _panelLogicVisibility { get; set; }
        public Visibility LogicVisibility
        {
            get
            {
                return this._panelLogicVisibility;
            }
            set
            {
                this._panelLogicVisibility = value;
                OnPropertyChanged("LogicVisibility");
            }
        }

        private Visibility _panelFileVisibilty;
        public Visibility PanelFileVisibility
        {
            get
            {
                return this._panelFileVisibilty;
            }

            set
            {
                this._panelFileVisibilty = value;
                OnPropertyChanged("PanelFileVisibility");
            }
        }

        private Visibility _panelCounterVisibilty;
        public Visibility PanelCounterVisibility
        {
            get
            {
                return this._panelCounterVisibilty;
            }

            set
            {
                this._panelCounterVisibilty = value;
                OnPropertyChanged("PanelCounterVisibility");
            }
        }


        private Visibility _panelDateVisibility;
        public Visibility PanelDateVisibility
        {
            get
            {
                return this._panelDateVisibility;
            }

            set
            {
                this._panelDateVisibility = value;
                OnPropertyChanged("PanelDateVisibility");
            }
        }

        private Visibility _panelTimeVisibility;
        public Visibility PanelTimeVisibility
        {
            get
            {
                return this._panelTimeVisibility;
            }

            set
            {
                this._panelTimeVisibility = value;
                OnPropertyChanged("PanelTimeVisibility");
            }
        }


        private int _indexLogic;
        public int Index_Logic
        {
            get { return _indexLogic; }
            set
            {
                setDefaultPanels();
                _indexLogic = value;

                OnPropertyChanged("Index_Logic");

                setPanelsVrLogics();
            }
        }

        private void setPanelsVrLogics()
        {

            ConfigCounter.Clear();
            ConfigDate.Clear();
            Config_File.Clear();
            ConfigTime.Clear();


            if (Index_Logic == 0)
                PanelDateVisibility = Visibility.Visible;
            else
            if (Index_Logic == 1)
                PanelTimeVisibility = Visibility.Visible;
            else
            if (Index_Logic == 2)
                PanelFileVisibility = Visibility.Visible;
            else
               if (Index_Logic == 3)
                PanelCounterVisibility = Visibility.Visible;
        }

        private void setDefaultPanels()
        {
            PanelDateVisibility = PanelCounterVisibility = PanelFileVisibility = PanelTimeVisibility =  Visibility.Collapsed;
        }

        #endregion

        private StringBuilder str_Tmp;
        private ObservableCollection<Layout> layout;
        private int indexLayout;
        private int idLayout;
        private View.EditField vField;

        private int indexField;
        private bool isField;

        public VmField(ObservableCollection<Layout> layouts, string id, View.EditField viewField)
        {
            vField = viewField;
            layout = layouts;
            
            try
            {
                idLayout = Regexp.GetIdLayout(id);
                indexLayout = Query.GetIndexLayout(layout, (int)idLayout);
                indexField = Query.GetIndexField(idLayout, id);

                var nextId = Query.GetNextIdField(idLayout);

                isField = indexField != -1;

                _layout_tmp = Query.GetLayout($"{idLayout}");

                viewField.nX.Maximum = _layout_tmp.Width - 6;
                viewField.nY.Maximum = _layout_tmp.Height - 6;

                if (isField)
                {
                    _field_tmp = Query.GetField(idLayout, indexField);
                }
                else
                {
                    _field_tmp = new Field() { Id = $"{_layout_tmp.Id}_{nextId}"};
                }

                str_Tmp = new StringBuilder();
                setDefaultPanels();
                _config_counter = new Counter();
                _config_date = new Date();
                _config_file = new File();
                _config_Test = new Test();
                _config_time = new Time();

                _config_Test.File_Full = $"{Global_Variable.PathLayout}\\{_layout_tmp.Id}\\{ _layout_tmp.File}";
                Index_Type = _field_tmp.Type == "Lógico" ? 0 : 1;
                Index_Logic = -1;

                ClearConfigDate = new Command(this.clearConfigDate, () => { return true; });
                ClearConfigTime = new Command(this.clearConfigTime, () => { return true; });
                ClearConfigCounter = new Command(this.clearConfigCounter, () => { return true; });
                ClearConfigFile = new Command(this.clearConfigFile, () => { return true; });
                DefineForeground = new Command(this.defineForeground, () => { return true; });
                DefineTestPage = new Command(this.defineTestPage, () => { return true; });
                TestField = new Command(this.testField, () => { return validateTest(); });
                SaveField = new Command(this.saveField, () => { return validateField(); });


                //O botão para adicionar a data sempre ficará ativo
                this.AddDate = new Command(this.AddExpressionDate, () => { return Index_Type == 0; });

                //O botão para adicionar a data sempre ficará ativo
                this.AddTime = new Command(this.AddExpressionTime, () => { return Index_Type == 0; });

                //Command para o botão de adicionar um contador
                this.AddCounter = new Command(this.AddExpressionCounter, () => { return (!string.IsNullOrWhiteSpace(ConfigCounter.Count_Field)); });

                this.AddFile = new Command(this.AddExpressionFile, () => { return (!string.IsNullOrWhiteSpace(Config_File.SensitiveCase)) && Index_Type == 0; });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        #region Validate Field

        private bool validateField()
        {
                                                                               
            if (Field_Tmp.Type == "Lógico")
            {
                return (!string.IsNullOrWhiteSpace(Field_Tmp.Name)) && (!string.IsNullOrWhiteSpace(Field_Tmp.Expression));
            }
            else
            {
                var barcodeValid = Regexp.ExpressionExists(Field_Tmp.Expression, "^<code_c_(Page|Field|fileSplit)<{0:0+}>>$");
                return barcodeValid && (!string.IsNullOrWhiteSpace(Field_Tmp.Name));
            }
        }

        private bool validateTest()
        {

            if (Field_Tmp.Type == "Lógico")
            {
                return (!string.IsNullOrWhiteSpace(Field_Tmp.Expression));
            }
            else
            {
                var barcodeValid = Regexp.ExpressionExists(Field_Tmp.Expression, "^<code_c_(Page|Field|fileSplit)<{0:0+}>>$");
                return barcodeValid;
            }
        }

        #endregion

        #region Save

        private void saveField()
        {
            string nameLayout = Layout_Tmp.Name;
            string alter = string.Empty;

            Layout layout_aux = Query.GetLayout($"{idLayout}");

            if (isField)
            {
                alter = "Editado";

                layout_aux.Fields.RemoveAt(indexField);

                layout_aux.Fields.Insert(indexField, _field_tmp);

                layout.RemoveAt(indexLayout);

            }
            else
            {
                alter = "Adicionado";
                layout_aux.Fields.Add(_field_tmp);
                layout.RemoveAt(indexLayout);

            }

            layout.Insert(indexLayout, layout_aux);

            var fullNameLayout = Query.GetLayoutName(idLayout);

            Json.SaveObject(fullNameLayout, layout[indexLayout]);

            vField.msgSaveField(_field_tmp, nameLayout, alter);

        }

        #endregion

        private void testField()
        {
            try
            {
                Layout layout_tmp;

                if (Confi_Test.TestAllFields == false)
                {

                    layout_tmp = null;

                    layout_tmp = new Layout()
                    {
                        File = Confi_Test.File
                    };

                    layout_tmp.Fields.Add(new Field
                    {
                        Display = _field_tmp.Display,
                        Font = _field_tmp.Font,
                        Position_x = _field_tmp.Position_x,
                        Position_y = _field_tmp.Position_y,
                        Rotation = _field_tmp.Rotation,
                        Expression = _field_tmp.Expression,
                        Type = _field_tmp.Type,

                    });
                }
                else
                {
                    layout_tmp = Query.GetLayout($"{idLayout}");

                    if (Confi_Test.TestAllFields && isField)
                    {
                        layout_tmp.Fields.RemoveAt(indexField);
                        layout_tmp.Fields.Insert(indexField, new Field
                        {
                            Display = _field_tmp.Display,
                            Font = _field_tmp.Font,
                            Position_x = _field_tmp.Position_x,
                            Position_y = _field_tmp.Position_y,
                            Rotation = _field_tmp.Rotation,
                            Expression = _field_tmp.Expression,
                            Type = _field_tmp.Type,
                        });
                    }
                    else
                    {
                        layout_tmp.Fields.Add(new Field
                        {
                            Display = _field_tmp.Display,
                            Font = _field_tmp.Font,
                            Position_x = _field_tmp.Position_x,
                            Position_y = _field_tmp.Position_y,
                            Rotation = _field_tmp.Rotation,
                            Expression = _field_tmp.Expression,
                            Type = _field_tmp.Type,

                        });
                    }

                }


                PDF.Split.IncludeFields(Confi_Test.File_Full, layout_tmp);

                //SyncFusionPDF.IncludeFields(Confi_Test.File_Full, layout_tmp);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region AddExpression
        private void AddExpressionCounter()
        {
            str_Tmp.Clear();

            if (Index_Type == 1 || Index_Type == 0 && Regexp.ExpressionExists(Field_Tmp.Expression, Regexp.FieldCode))
                Field_Tmp.Expression = "";

            str_Tmp.Append(Index_Type == 1 ? "<code_" : "<");

            if (ConfigCounter.Count_Field == "Página")
            {
                str_Tmp.Append($"c_Page<{ConfigCounter.MaskDigit}>>");
            }
            else
            if (ConfigCounter.Count_Field == "Campo")
            {
                str_Tmp.Append($"c_Field<{ConfigCounter.MaskDigit}>>");
            }
            else
            if (ConfigCounter.Count_Field == "Arquivo Split")
            {
                str_Tmp.Append($"c_fileSplit<{ConfigCounter.MaskDigit}>>");
            }

            Field_Tmp.Expression += $"{str_Tmp}";
        }

        private void AddExpressionDate()
        {
            str_Tmp.Clear();

            if (Index_Type == 1 || Index_Type == 0 && Regexp.ExpressionExists(Field_Tmp.Expression, Regexp.FieldCode))
                Field_Tmp.Expression = "";

            var fDays = ConfigDate.WorkDay == true ? "U" : "N";

            Field_Tmp.Expression += ($"<s_Date_{ConfigDate.IncDays}{fDays}<{ConfigDate.MaskFormat}>>");

        }

        private void AddExpressionTime()
        {
            str_Tmp.Clear();

            if (Index_Type == 1 || Index_Type == 0 && Regexp.ExpressionExists(Field_Tmp.Expression, Regexp.FieldCode))
                Field_Tmp.Expression = "";

            Field_Tmp.Expression += ($"<s_Time<{ConfigTime.MaskFormat}>>");
        }



        private void AddExpressionFile()
        {
            str_Tmp.Clear();

            if (Index_Type == 1 || Index_Type == 0 && Regexp.ExpressionExists(Field_Tmp.Expression, Regexp.FieldCode))
                Field_Tmp.Expression = "";

            Field_Tmp.Expression += ($"<s_fileName<{Config_File.MaskFile}>>");
        }

        #endregion

        #region Define
        private void defineTestPage()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Title = "Escolha um arquivo PDF";
            openFile.ValidateNames = true;
            openFile.InitialDirectory = @"c:\";
            openFile.Filter = "Arquivo PDF|*.pdf";

            Nullable<bool> result = openFile.ShowDialog();

            if (result == true)
                Confi_Test.File_Full = openFile.FileName;
        }

        private void defineForeground()
        {
            ColorDialog cd = new ColorDialog();

            var result = cd.ShowDialog();

            if (result == DialogResult.OK)
            {
                Field_Tmp.Font.Red = cd.Color.R;
                Field_Tmp.Font.Green = cd.Color.G;
                Field_Tmp.Font.Blue = cd.Color.B;
            }
        }

        #endregion
    }
}
