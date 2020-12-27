using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ZH_Mole
{
    public class Mole
    {
        public int Number { get; private set; }
        public int Health { get; private set; }
        public Mole(int num)
        {
            Number = num;
            Health = 2;
        }
        public void damage()
        {
            Health = Health - 1;
        }

        public bool Died { get { return Health == 0; } private set { Died = value; } }
    }
    public class Model
    {
        private int _gameTime;
        public List<Mole> moles;
        public List<Mole> lastKilledMoles;
        public int[,] map;
        public int Size { get; set; }
        public bool IsGameOver { get { return _gameTime == 0; } set { IsGameOver = value; } }
        public EventHandler generateTable;
        public EventHandler GameAdvanced;
        public int Points {get; private set; }
        public Model()
        {
            Size = 0;
            _gameTime = 0;
            moles = new List<Mole>();
            lastKilledMoles = new List<Mole>();
            map = new int[5, 5];
        }

        public int getTime()
        {
            return _gameTime;
        }

        public void StartGame()
        {
            OnGenerateTable();
            Size = 5;
            _gameTime = 30;
            Points = 0;
        }

        private void OnGenerateTable()
        {
            if (generateTable != null)
            {
                generateTable(this, EventArgs.Empty);
            }
        }

        public void StepGame()
        {
            DamageMoles();
            Random rnd = new Random();
            int number = rnd.Next(0, 101);
            int newmoles = 0;

            if (number < 41)
            {
                newmoles = 2;
            }
            else if (number < 81)
            {
                newmoles = 1;
            }

            for (int i = 0; i < newmoles; ++i)
            {
                bool goodposition = false;

                while (!goodposition)
                {
                    bool ok = true;
                    int pos = rnd.Next(0, 25);
                    foreach (Mole m in moles)
                    {
                        if (m.Number == pos) ok = false;
                    }
                    foreach(Mole m in lastKilledMoles)
                    {
                        if (m.Number == pos) ok = false;
                    }
                    if (ok && pos % 2 == 0)
                    {
                        goodposition = true;
                        moles.Add(new Mole(pos));
                    }
                }

            }
            //MessageBox.Show(newmoles.ToString());
            genMap();
            lastKilledMoles.Clear();
        }

        private void genMap()
        {
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    map[i, j] = 0;
                }
            }

            foreach (Mole m in moles)
            {
                int x = m.Number / 5;
                int y = m.Number % 5;
                map[x, y] = 1;
            }
        }

        private void DamageMoles()
        {
            foreach(Mole m in moles)
            {
                m.damage();
            }

            moles.RemoveAll(e => e.Died);
        }

        public int getMapElem(int x, int y)
        {
            return map[x, y];
        }

        public void AdvanceTime()
        {
            if (IsGameOver)
                return;

            StepGame();

            _gameTime--;
            OnGameAdvanced();

            if (_gameTime == 0)
            {
                //end the game
            }
        }

        private void OnGameAdvanced()
        {
            if (GameAdvanced != null)
            {
                GameAdvanced(this, EventArgs.Empty);
            }
        }

        public void Kill(int num)
        {
            int beforeCount = moles.Count;
            moles.RemoveAll(e => e.Number == num);
            lastKilledMoles.Add(new Mole(num));
            if(moles.Count == beforeCount)
            {
                Points -= 1;
            }
            else
            {
                Points += 1;
            }
            genMap();
        }

    }
}
