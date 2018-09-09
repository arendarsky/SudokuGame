using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sudoku.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        internal class Counter
        {
            public int Value { get; set; }
            public Counter(int value)
            {
                Value = value;
            }
        }

        public ActionResult Index()
        {
            if (TempData.ContainsKey("Score"))
            {
                var c = TempData["Score"] as Counter;
                ViewBag.Score = c.Value;
                TempData["Score"] = c;
            }
            else
            {
                var c = new Counter(0);
                ViewBag.Score = c.Value;
                TempData["Score"] = c;
            }
            if (TempData.ContainsKey("Table"))
                TempData.Remove("Table");
            if (TempData.ContainsKey("NewTable"))
                TempData.Remove("NewTable");
            return View();
        }

        [HttpPost]
        public ActionResult CreateTheTable(int level)
        {
            SudokuTable Table = new SudokuTable(level);
            TempData["Table"] = Table;
            return RedirectToAction("ViewTheTable");
        }


        [HttpGet]
        public ActionResult ViewTheTable()
        {
            SudokuTable Table;
            if(TempData.ContainsKey("NewTable"))
            {
                Table = TempData["NewTable"] as SudokuTable;
                TempData["NewTable"] = Table;
            }
            else
            {
                Table = TempData["Table"] as SudokuTable;
                TempData["Table"] = Table;
            }
            var c = TempData["Score"] as Counter;
            ViewBag.Score = c.Value;
            TempData["Score"] = c;
            return View(Table);
        }


        [HttpPost]
        public ActionResult CheckTheTable(List<int> Value)
        {
            SudokuTable NewSudokuTable = new SudokuTable();
            int c = 0;
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = i*3; k < 3*(i +1); k++)
                    {
                        for (int m = j*3; m < 3*(j+1); m++)
                        {
                            if (Value[c] == 0)
                                NewSudokuTable.Values[k, m].Hidden = true;
                            else
                                NewSudokuTable.Values[k , m].Value = Value[c];
                            c++;
                        }
                    }
                }
            }           
            SudokuTable OldTable = TempData["Table"] as SudokuTable;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (OldTable.Values[i, j].Hidden)
                        NewSudokuTable.Values[i, j].IsEditable = true;
                }
            TempData["Table"] = OldTable;
            NewSudokuTable.CheckIfTheTableIsCorrect();
            if (!NewSudokuTable.IsNotCorrect)
            {
                var ct = TempData["Score"] as Counter;
                ct.Value++;
                TempData["Score"] = ct;
                return RedirectToAction("Win");
            }
            else
            {
                TempData["NewTable"] = NewSudokuTable;
                return RedirectToAction("ViewTheTable");
            }
        }
        public ActionResult ClearTheTable()
        {
            TempData.Remove("NewTable");
            return RedirectToAction("ViewTheTable");
        } 
        public ActionResult Win()
        {
            return View();
        }
    }
}