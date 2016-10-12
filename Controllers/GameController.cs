using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace tictactoe.Controllers
{
    public class GameController : Controller
    {

        //route a request to make a 
        public IActionResult Index()
        {
            Models.Board board = new Models.Board();
            board.Init(5);
            return View(board);
        }
       
        [HttpGetAttribute("/game/startgame/{num}")]
        public int[,] StartGame(int num){
            Models.Board board = new Models.Board();
            board.Init(num);
            return board.tiles;
        }
        
        [HttpGetAttribute("/game/play/{str}/{row}/{col}")]
        public Tuple<string,int[,]> Play(string str, string row, string col){
            Models.Board board = new Models.Board();
            board.Deserialize(str);
            board.PlayerMakeMove(Int32.Parse(row),Int32.Parse(col),1);
            string outcome = board.CheckGameOutcome();
            if(outcome == ""){
                board.ComputerMakeMove(-1);
                outcome = board.CheckGameOutcome();
            }
            return Tuple.Create(outcome,board.tiles);
        }
        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
        
    }
}
