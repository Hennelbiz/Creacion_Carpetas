using AlumnosCarpetas.Model;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlumnosCarpetas.ViewModel;

public class MasterView : BindableBase
{

    private string rutaDirectorio;

    public string RutaDirectorio
    {
        get
        {
            return rutaDirectorio;
        }
        set
        {
            if (rutaDirectorio == value)
            {
                return;
            }
            rutaDirectorio = value;
            RaisePropertyChanged();
        }
    }

    private bool formatoNombre1;

    public bool FormatoNombre1
    {
        get
        {
            return formatoNombre1;
        }
        set
        {
            if (formatoNombre1 == value)
            {
                return;
            }
            formatoNombre1 = value;
            RaisePropertyChanged();
        }
    }

    private bool formatoNombre2;

    public bool FormatoNombre2
    {
        get
        {
            return formatoNombre2;
        }
        set
        {
            if (formatoNombre2 == value)
            {
                return;
            }
            formatoNombre2 = value;
            RaisePropertyChanged();
        }
    }

    private ObservableCollection<NombreAlumno> items;

    public ObservableCollection<NombreAlumno> Items
    {
        get
        {
            return items;
        }
        set
        {
            if (items == value)
            {
                return;
            }
            items = value;
            RaisePropertyChanged();
        }
    }

    public ICommand ObtenerDirectorioCommand { get; set; }
    public ICommand LecturaArchivoCSVCommand { get; set; }
    public ICommand GenerarCarpetasCommand { get; set; }

    public MasterView()
    {
        RutaDirectorio = string.Empty;
        FormatoNombre1 = true;
        ObtenerDirectorioCommand = new DelegateCommand(ObtenerDirectorioCommnadExecute);
        LecturaArchivoCSVCommand = new DelegateCommand(LecturaArchivoCSVCommandExecute);
        GenerarCarpetasCommand = new DelegateCommand(GenerarCarpetasCommandExecute, GenerarCarpetasCommandCanExecute).ObservesProperty(() => Items);
    }

    public void ObtenerDirectorioCommnadExecute()
    {
        var dialog = new Microsoft.Win32.SaveFileDialog();
        dialog.Title = "Select a Directory";
        dialog.Filter = "Directory|*.this.directory";
        dialog.FileName = "select";

        if (dialog.ShowDialog() == true)
        {
            string path = dialog.FileName;
            path = path.Replace("\\select.this.directory", "");
            path = path.Replace(".this.directory", "");

            // If user has changed the filename, create the new directory
            //if (!System.IO.Directory.Exists(path))
            //{
            //    System.IO.Directory.CreateDirectory(path);
            //}

            RutaDirectorio = path;
        }
    }

    public void LecturaArchivoCSVCommandExecute()
    {
        StringBuilder errores = new StringBuilder();

        try
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "CSV files(*.csv) | *.csv",
            };

            if (ofd.ShowDialog() == true)
            {
                string line = string.Empty;

                if (Items != null)
                    Items.Clear();
                else
                    Items = new ObservableCollection<NombreAlumno>();

                using (var sr = new StreamReader(ofd.FileName, Encoding.UTF8))
                {
                    line = sr.ReadLine()!;
                    char delimiter = ',';
                    var fila = new StringBuilder();

                    while (line != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var array = line.Split(delimiter);

                            var alumno = new NombreAlumno();

                            if (array.Length > 2 && !string.IsNullOrWhiteSpace(array[3].ToString()))
                            {
                                alumno.PrimerNombre = FormatoNombre1 ? array[0] : array[3];
                                alumno.SegundoNombre = FormatoNombre1 ? array[1] : array[2];
                                alumno.ApellidoPaterno = FormatoNombre1 ? array[2] : array[1];
                                alumno.ApellidoMaterno = FormatoNombre1 ? array[3] : array[0];

                                if (alumno.PrimerNombre.Contains('Ñ'))
                                    _ = alumno.PrimerNombre.Replace('Ñ', 'N').Replace('ñ', 'n');
                                if (alumno.SegundoNombre.Contains('Ñ'))
                                    _ = alumno.SegundoNombre.Replace('Ñ', 'N').Replace('ñ', 'n');
                                if (alumno.ApellidoPaterno.Contains('Ñ'))
                                    _ = alumno.ApellidoPaterno.Replace('Ñ', 'N').Replace('ñ', 'n');
                                if (alumno.ApellidoMaterno.Contains('Ñ'))
                                    _ = alumno.ApellidoMaterno.Replace('Ñ', 'N').Replace('ñ', 'n');
                            }
                            else
                            {
                                alumno.PrimerNombre = FormatoNombre1 ? array[0] : array[2];
                                alumno.ApellidoPaterno = FormatoNombre1 ? array[1] : array[1];
                                alumno.ApellidoMaterno = FormatoNombre1 ? array[2] : array[0];

                                if (alumno.PrimerNombre.Contains('Ñ'))
                                    _ = alumno.PrimerNombre.Replace('Ñ', 'N').Replace('ñ', 'n');
                                if (alumno.ApellidoPaterno.Contains('Ñ'))
                                    _ = alumno.ApellidoPaterno.Replace('Ñ', 'N').Replace('ñ', 'n');
                                if (alumno.ApellidoMaterno.Contains('Ñ'))
                                    _ = alumno.ApellidoMaterno.Replace('Ñ', 'N').Replace('ñ', 'n');
                            }

                            Items.Add(alumno);
                        }
                        line = sr.ReadLine()!;
                    }
                    sr.Close();
                }
            }
            else
                return;
        }
        catch (Exception ex)
        {
            errores.AppendLine(ex.Message);
        }
        finally { }
    }

    public bool GenerarCarpetasCommandCanExecute()
    {
        return Items != null;
    }

    public void GenerarCarpetasCommandExecute()
    {
        try
        {
            foreach (var item in Items)
            {
                string rutaBase = RutaDirectorio;
                string delimitador = "\\";
                string datosAlumno = string.Empty;
                if(!string.IsNullOrWhiteSpace(item.SegundoNombre))
                    datosAlumno = $"{item.PrimerNombre?.Trim()} {item.SegundoNombre?.Trim()} {item.ApellidoPaterno?.Trim()} {item.ApellidoMaterno?.Trim()}";
                else
                    datosAlumno = $"{item.PrimerNombre?.Trim()} {item.ApellidoPaterno?.Trim()} {item.ApellidoMaterno?.Trim()}";

                string pathCarpeta = $"{rutaBase}{delimitador}{datosAlumno}";

                if (!Directory.Exists(pathCarpeta))
                {
                    Directory.CreateDirectory(pathCarpeta);
                }
            }
        }
        catch (Exception ex)
        {

            throw;
        }
        finally { }
    }
}