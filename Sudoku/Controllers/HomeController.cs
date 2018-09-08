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

        public ActionResult Index()
        {
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
            return View(Table);
        }


        [HttpPost]
        public ActionResult CheckTheTable(List<int> Value)
        {
            SudokuTable NewSudokuTable = new SudokuTable();
            int i = 0;
            foreach (var v in Value)
            {
                if (v == 0)
                    NewSudokuTable.Values[i / 9, i % 9].Hidden = true;
                else
                    NewSudokuTable.Values[i / 9, i % 9].Value = v;
                i++;
            }
            SudokuTable OldTable = TempData["Table"] as SudokuTable;
            for (i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (OldTable.Values[i, j].Hidden)
                        NewSudokuTable.Values[i, j].IsEditable = true;
                }
            TempData["NewTable"] = NewSudokuTable;
            TempData["Table"] = OldTable;
            return Redirect("ViewTheTable");
        }

    }
}