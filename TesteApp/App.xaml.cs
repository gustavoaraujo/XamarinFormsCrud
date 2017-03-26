using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CrudBasicoXamarinForms
{
	public partial class App : Application
	{
		public List<Pessoa> Pessoas;
		public Entry inputNome, inputSobrenome;
		public Pessoa pessoaSelecionada;

		public App()
		{
			InitializeComponent();

			Pessoas = new List<Pessoa>();
			//MainPage = new CrudBasicoXamarinFormsPage();
			MainPage = new ContentPage
			{
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
					Children = { StackInicial() }
				}
			};
		}

		public StackLayout StackInicial(string nome = "", string sobrenome = "")
		{
			inputNome = new Entry
			{
				Placeholder = "Nome",
				Text = nome
			};

			inputSobrenome = new Entry
			{
				Placeholder = "Sobrenome",
				Text = sobrenome
			};

			var stack = new StackLayout();
			stack.Children.Add(inputNome);
			stack.Children.Add(inputSobrenome);

			if(pessoaSelecionada == null) 
			{
				var botaoCadastro = new Button
				{
					Text = "Cadastrar",
					HorizontalOptions = LayoutOptions.Center
				};

				botaoCadastro.Clicked += OnCadastroClicked;
				stack.Children.Add(botaoCadastro);
			} 
			else 
			{
				var stackMenuAlterar = new StackLayout
				{
					HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
				};

				var botaoAlterar = new Button
				{
					Text = "Alterar",
					VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
				};

				botaoAlterar.Clicked += OnAlteracaoClicked;

				var botaoDeletar = new Button
				{
					Text = "Deletar",
					VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
				};

				botaoDeletar.Clicked += OnDeleteClicked;

				stackMenuAlterar.Children.Add(botaoAlterar);
				stackMenuAlterar.Children.Add(botaoDeletar);
				stack.Children.Add(stackMenuAlterar);
			}

			return stack;
		}

		public StackLayout StackLista()
		{
			var listaLabels = new List<Label>();
			foreach (var pessoa in Pessoas) 
			{
				var label = new Label
				{
					Text = pessoa.Nome + " " + pessoa.Sobrenome
				};

				var tgr = new TapGestureRecognizer();
				tgr.Tapped += (sender, e) => OnPessoaSelecionada(sender, e, pessoa);
				label.GestureRecognizers.Add(tgr);

				listaLabels.Add(label);
			} 

			var botaoVoltar = new Button
			{
				Text = "Voltar",
				HorizontalOptions = LayoutOptions.Center
			};

			botaoVoltar.Clicked += OnVoltarClicked;

			var stack = new StackLayout();
			foreach (var label in listaLabels)
				stack.Children.Add(label);
			stack.Children.Add(botaoVoltar);

			return stack;
		}

		void OnDeleteClicked(object sender, EventArgs e) 
		{
			pessoaSelecionada = null;

			IrParaPaginaLista();			
		}

		void OnPessoaSelecionada(object sender, EventArgs e, Pessoa p) 
		{
			Pessoas.Remove(p);
			pessoaSelecionada = p;

			MainPage = new ContentPage
			{
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
					Children = { StackInicial(p.Nome, p.Sobrenome) }
				}
			};
		}

		async void OnCadastroClicked(object sender, EventArgs e) 
		{
			if(string.IsNullOrEmpty(inputNome.Text) || string.IsNullOrEmpty(inputSobrenome.Text)) 
			{
				await App.Current.MainPage.DisplayAlert("Erro", "Todos os elementos devem ser preenchidos.", "OK");
				return;
			}

			Pessoa p = new Pessoa { Nome = inputNome.Text, Sobrenome = inputSobrenome.Text };
			Pessoas.Add(p);

			IrParaPaginaLista();		
		}

		async void OnAlteracaoClicked(object sender, EventArgs e) 
		{
			if(string.IsNullOrEmpty(inputNome.Text) || string.IsNullOrEmpty(inputSobrenome.Text)) 
			{
				await App.Current.MainPage.DisplayAlert("Erro", "Todos os elementos devem ser preenchidos.", "OK");
				return;
			}

			pessoaSelecionada.Nome = inputNome.Text;
			pessoaSelecionada.Sobrenome = inputSobrenome.Text;
			Pessoas.Add(pessoaSelecionada);

			pessoaSelecionada = null;

			IrParaPaginaLista();		
		}

		private void IrParaPaginaLista() 
		{
			MainPage = new ContentPage
			{
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
					Children = { StackLista() }
				}
			};
		}

		void OnVoltarClicked(object sender, EventArgs e)
		{
			MainPage = new ContentPage
			{
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Start,
					Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
					Children = { StackInicial() }
				}
			};
		}


		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
