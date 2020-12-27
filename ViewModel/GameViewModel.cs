using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ZH_Mole.ViewModel
{
    public class GameViewModel : ViewModelBase
    {
        public ObservableCollection<Field> Fields { get; set; }
        public string Time { get { return _model.getTime().ToString() + " seconds"; } private set { Time = value; } }
        public string Points { get { return _model.Points.ToString(); } private set { Points = value; } }
        private Model _model;

        public DelegateCommand StartGameCommand { get; private set; }

        public EventHandler StartGame;

        public GameViewModel(Model m)
        {
            _model = m;
            Fields = new ObservableCollection<Field>();
            StartGameCommand = new DelegateCommand(
                (_) => OnStartGame()
            );
            _model.generateTable += new EventHandler(GenerateTable);
            _model.GameAdvanced += new EventHandler(Model_GameAdvanced);
        }

        private void Model_GameAdvanced(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void OnStartGame()
        {
            if (StartGame != null)
                StartGame(this, EventArgs.Empty);
        }

        private void GenerateTable(object sender, EventArgs e)
        {
            Fields.Clear();
            for (Int32 i = 0; i < 5; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 5; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = String.Empty,
                        Color = _model.getMapElem(i, j) == 0 ? "White" : "Black",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => FieldClicked(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");

        }

        private void FieldClicked(int num)
        {
            if (_model.getTime() == 0) return;
            Field field = Fields[num];

            _model.Kill(num);
            RefreshTable();
        }

        private void RefreshTable()
        {
            Fields.Clear();
            for (Int32 i = 0; i < 5; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < 5; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = String.Empty,
                        Color = _model.getMapElem(i, j) == 0 ? "White" : "Black",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => FieldClicked(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
            OnPropertyChanged("Time");
            OnPropertyChanged("Points");
        }
    }
}
