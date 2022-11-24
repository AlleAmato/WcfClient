using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertitoreValute.ConvertitoreService;
using System.ComponentModel.DataAnnotations;

namespace ConvertitoreValute.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
		private float _importo;

		[Range(0,float.MaxValue,ErrorMessage ="Importo negativo")]
		[Required(ErrorMessage ="Campo obbligatorio")]
		public float Importo
		{
			get { return _importo; }
			set 
			{
				/*if (value < 0)
				{
					throw new ArgumentException();
				}*/
				_importo = value; 
			}
		}

		private string _importoConvertito;

		public string ImportoConvertito
		{
			get { return _importoConvertito; }
			set { _importoConvertito = value; NotifyPropertyChanged("ImportoConvertito"); }
		}


		private List<string> _valute;

		public List<string> Valute
		{
			get { return _valute; }
			set { _valute = value; }
		}

		private string _valutaDa;

		public string ValutaDa
		{
			get { return _valutaDa; }
			set { _valutaDa = value; Converti(); NotifyPropertyChanged("ValutaDa"); }
		}

		private string _valutaA;

		public string ValutaA
		{
			get { return _valutaA; }
			set { _valutaA = value; Converti(); NotifyPropertyChanged("ValutaA"); }
		}

		private string _error;
		public string Error => _error;


		/*private string _importoErrore;

		public string ImportoErrore
		{
			get { return _importoErrore; }
			set { _importoErrore = value; NotifyPropertyChanged("ImportoErrore"); }
		}*/

		public string this[string columnName]
		{
			get
			{
				ValidationContext valContext = new ValidationContext(this)
				{
					MemberName = columnName
				};

				List<ValidationResult> validationResults = new List<ValidationResult>();

				if (Validator.TryValidateProperty(
						GetType().GetProperty(columnName).GetValue(this),
						valContext,
						validationResults) )
					return "";

				return validationResults.First().ErrorMessage;
            }
		}

		public MainWindowViewModel()
		{
			Valute = new List<string>
			{
				"EUR","ITL","DEM","FRF"
			};
			_valutaDa = "EUR";
            _valutaA = "EUR";
			Converti();
        }

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string propName)
		{ 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		internal void Converti()
		{
			ConvertitoreServiceClient client = new ConvertitoreServiceClient();

			try
			{
				float conversione = client.Converti(Importo, ValutaDa, ValutaA);
				ImportoConvertito = $"{conversione} {ValutaA}";
			}
			catch(Exception)
			{
                client.Close();
				throw;
            }
        }

		internal void ScambiaValute()
		{
			string temp = ValutaDa;
			ValutaDa = ValutaA;
			ValutaA = temp;
            Converti();
        }
	}
}
