using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core
{
    public class SudokuTable
    {
        const int _size = 9;
        public int[,] Values { get; set; }
        public int Difficulty { get; set; }


        public SudokuTable(int difficulty)
        {
            Difficulty = difficulty;
            Values = new int[_size, _size];
            GenerateTable();
        }


        private void GenerateTable()
        {
            GenerateStandartLayout();
            Random r = new Random();
            int repeats = r.Next(10, 20);
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
                    Values[i, j] = (i * n + i / n + j) % (n * n) + 1;
            
        }


        private void Transposing()
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < i; j++)
                {
                    int tmp = Values[i, j];
                    Values[i, j] = Values[j, i];
                    Values[j, i] = tmp;
                }
        }


        private void ChangeRows(int firstRow, int secondRow)
        {
            for (int i = 0; i < _size; i++)
            {
                int tmp = Values[firstRow, i];
                Values[firstRow, i] = Values[secondRow, i];
                Values[firstRow, i] = tmp;
            }
        }


        private void ChangeColumns(int firstColumn, int secondColumn)
        {
            for (int i = 0; i < _size; i++)
            {
                int tmp = Values[i, firstColumn];
                Values[i, firstColumn] = Values[i, secondColumn];
                Values[i, secondColumn] = tmp;
            }
        } 


        private void ChangeValues(int firstValue, int secondValue)
        {
            int tmp = firstValue;
            firstValue = secondValue;
            secondValue = tmp;
        }
    }
}
