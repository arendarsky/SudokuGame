using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class Cell
    {
        public int Value { get; set; }
        public bool Hidden { get; set; }
        public bool IsEditable { get; set; }
        public bool Wrong { get; set; }
        public Cell(int value)
        {
            Value = value;
            Hidden = false;
            IsEditable = false;
            Wrong = false;
        }
        public Cell()
        {
            Hidden = false;
            IsEditable = false;
            Wrong = false;
        }
    }
    public class SudokuTable
    {
        
        const int _size = 9;
        public Cell[,] Values { get; set; }
        public int Difficulty { get; set; }
        public bool IsNotCorrect { get; set; }


        public SudokuTable(int difficulty)
        {
            Difficulty = difficulty;
            Values = new Cell[_size, _size];
            GenerateTable();
            RandomizeHiddenValues();
        }
        public SudokuTable()
        {
            Values = new Cell[_size, _size];
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    Values[i, j] = new Cell();
        }


        public void CheckIfTheTableIsCorrect()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (Values[i, j].Value == 0)
                    {
                        IsNotCorrect = true;
                        continue;
                    }
                    for (int k = j + 1; k < _size; k++)
                        if (Values[i, j].Value == Values[i, k].Value)
                        {
                            if (Values[i, j].IsEditable)
                                Values[i, j].Wrong = true;
                            if (Values[i, k].IsEditable)
                                Values[i, k].Wrong = true;
                            IsNotCorrect = true;
                        }
                    for (int k = i + 1; k < _size; k++)
                        if (Values[i, j].Value == Values[k, j].Value)
                        {
                            if (Values[i, j].IsEditable)
                                Values[i, j].Wrong = true;
                            if(Values[k, j].IsEditable)
                                Values[k, j].Wrong = true;
                            IsNotCorrect = true;
                        }
                }
            }
            for (int i = 0; i < _size/3; i++)
            {
                for (int j = 0; j < _size / 3; j++)
                {
                    for (int k = i * 3; k < _size / 3 * (1 + i); k++)
                    {
                        for (int m = j * 3; m < _size / 3 * (1 + j); m++)
                        {
                            if (Values[k, m].Value == 0)
                            {
                                IsNotCorrect = true;
                                continue;
                            }
                            for (int x = m+1; x < _size/3*(1+j); x++)
                            {
                                if (Values[k, m].Value == Values[k, x].Value)
                                {
                                    if (Values[k, m].IsEditable)
                                        Values[k, m].Wrong = true;
                                    if (Values[k, x].IsEditable)
                                        Values[k, x].Wrong = true;
                                    IsNotCorrect = true;
                                }
                            }
                            for (int c = k+1; c < _size/3*(1 + i); c++)
                            {
                                for (int x = j*3; x < _size/3*(1+j); x++)
                                {
                                    if (Values[k, m].Value == Values[c, x].Value)
                                    {
                                        if (Values[k, m].IsEditable)
                                            Values[k, m].Wrong = true;
                                        if (Values[c, x].IsEditable)
                                            Values[c, x].Wrong = true;
                                        IsNotCorrect = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void GenerateTable()
        {
            GenerateStandartLayout();
            Random r = new Random();
            int repeats = r.Next(100, 200);
            for (int i = 0; i < repeats; i++)
            {
                int operation = r.Next(1, 4);
                switch (operation)
                {
                    case 1:
                        Transposing();
                        break;
                    case 2:
                        int rowArea = r.Next(1, 4);
                        int firstRow;
                        int secondRow;
                        switch (rowArea)
                        {
                            case 1:
                                firstRow = r.Next(0, 3);
                                secondRow = r.Next(0, 3);
                                break;
                            case 2:
                                firstRow = r.Next(3, 6);
                                secondRow = r.Next(3, 6);
                                break;
                            default:
                                firstRow = r.Next(6, 9);
                                secondRow = r.Next(6, 9);
                                break;
                        }
                        ChangeRows(firstRow, secondRow);
                        break;
                    default:
                        int columnArea = r.Next(1, 4);
                        int firstColumn;
                        int secondColumn;
                        switch (columnArea)
                        {
                            case 1:
                                firstColumn = r.Next(0, 3);
                                secondColumn = r.Next(0, 3);
                                break;
                            case 2:
                                firstColumn = r.Next(3, 6);
                                secondColumn = r.Next(3, 6);
                                break;
                            default:
                                firstColumn = r.Next(6, 9);
                                secondColumn = r.Next(6, 9);
                                break;
                        }
                        ChangeColumns(firstColumn, secondColumn);
                        break;
                }
            }
        }


        private void GenerateStandartLayout()
        {
            int n = 3;
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    Values[i, j] = new Cell((i * n + i / n + j) % (n * n) + 1);
            
        }


        private void Transposing()
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < i; j++)
                {
                    int tmp = Values[i, j].Value;
                    Values[i, j].Value = Values[j, i].Value;
                    Values[j, i].Value = tmp;
                }
        }


        private void ChangeRows(int firstRow, int secondRow)
        {
            for (int i = 0; i < _size; i++)
            {
                int tmp = Values[firstRow, i].Value;
                Values[firstRow, i].Value = Values[secondRow, i].Value;
                Values[firstRow, i].Value = tmp;
            }
        }


        private void ChangeColumns(int firstColumn, int secondColumn)
        {
            for (int i = 0; i < _size; i++)
            {
                int tmp = Values[i, firstColumn].Value;
                Values[i, firstColumn].Value = Values[i, secondColumn].Value;
                Values[i, secondColumn].Value = tmp;
            }
        } 

        private void RandomizeHiddenValues()
        {
            int AmmountOfHiddenValues = _size * _size - Difficulty;
            Random r = new Random();
            do
            {
                int i = r.Next(0, _size);
                int j = r.Next(0, _size);
                    if (AmmountOfHiddenValues == 0)
                        break;
                        if (AmmountOfHiddenValues == 0)
                            break;
                        int Hide = r.Next(0, 2);
                        switch (Hide)
                        {
                            case 0:
                                break;
                            case 1:
                                if (!Values[i, j].Hidden)
                                {
                                    Values[i, j].Hidden = true;
                                    AmmountOfHiddenValues--;
                                    break;
                                }
                                else
                                    break;
                        }
            }
            while (AmmountOfHiddenValues > 0);
        }
    }
}
