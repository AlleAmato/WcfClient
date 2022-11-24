using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient.AthletesService;

namespace WCFClient.ViewModel
{
	internal class MainWindowViewModel
	{
		private List<AthletesNF> _listaAtleti;

		public List<AthletesNF> ListaAtleti
		{
			get { return _listaAtleti; }
			set { _listaAtleti = value; }
		}

		public MainWindowViewModel()
		{
			AthletesServiceClient client = new AthletesServiceClient();
			_listaAtleti = new List<AthletesNF>();
			_listaAtleti.Add(client.GetRandom());
			client.Close();
		}
    }
}
