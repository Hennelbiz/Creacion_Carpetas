using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlumnosCarpetas.Model;

public class NombreAlumno : BindableBase
{
    private string? primerNombre;

    public string? PrimerNombre
    {
        get
        {
            return primerNombre;
        }
        set
        {
            if (primerNombre == value)
            {
                return;
            }
            primerNombre = value;
            RaisePropertyChanged();
        }
    }

    private string? segundoNombre;

    public string? SegundoNombre
    {
        get
        {
            return segundoNombre;
        }
        set
        {
            if (segundoNombre == value)
            {
                return;
            }
            segundoNombre = value;
            RaisePropertyChanged();
        }
    }

    private string? apellidoPaterno;

    public string? ApellidoPaterno
    {
        get 
        {
            return apellidoPaterno; 
        }
        set 
        {
            if (apellidoPaterno == value)
            {

                return;
            }
            apellidoPaterno = value;
            RaisePropertyChanged();
        }
    }

    private string? apellidoMaterno;

    public string? ApellidoMaterno
    {
        get 
        {
            return apellidoMaterno; 
        }
        set 
        {
            if (apellidoMaterno == value)
            {
                return;
            }
            apellidoMaterno = value;
            RaisePropertyChanged();
        }
    }

}
