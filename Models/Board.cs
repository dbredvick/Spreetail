using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace tictactoe.Models
{
    public class Board 
    {
        public int dimensions {get; set;}
        public int[,] tiles {get; set;}
        public int[] sumRows {get; set;}
        public int[] sumCols {get; set;}
        public int sumDiag {get; set;}

        //funciton to make player moves
        public void PlayerMakeMove(int row, int col, int token){
            if(ValidMove(row,col)){
                tiles[row,col] = token;
            }
        }

        //function to check for game outcome - Player wins, Computer wins, cat, or game continues
        public string CheckGameOutcome(){
            string diag = CheckDiag();
            string diag2 = CheckDiag2();
            string rows;
            string cols;
            //if statements arranged in this order for maximum efficiency - most likely each move doens't result in win.
            if(diag2 == ""){
                if(diag == ""){
                    rows = CheckRows();
                    if(rows == "found zero" || rows == ""){
                        cols = CheckCols();

                        if(cols == ""){
                            if(rows == "found zero"){
                                //the game continues
                                return "";
                            }else{
                                return "Cat Game";
                            }
                        }else if(cols == "player"){
                            return "Player Wins";
                        }else if(cols == "computer"){
                            return "Computer Wins";
                        }
                    }else if(rows == "player"){
                        return "Player Wins";
                    }else if(rows == "computer"){
                        return "Computer Wins";
                    }
                }else if(diag == "player"){
                    return "Player Wins";
                }else if(diag == "computer"){
                    return "Computer Wins";
                }
            }else if(diag2 == "player"){
                return "Player Wins";
            }else if(diag2 == "computer"){
                return "Computer Wins";
            }
                return "";
            
        }

        public void Init(int size){
            //verifies that the dimensions are set before using them to create the board.
            if(dimensions == 0){
                dimensions = size;
            }
            tiles = new int[dimensions,dimensions];
            //set string array to empty values
            for(int i = 0; i < dimensions; i++){
                for(int j = 0; j < dimensions; j++){
                    tiles[i,j] = 0;
                }
            }
        }

        
        //function to make computer random move out of remaining tiles
        public bool ComputerMakeMove(int token){
            bool validChoice = false;
            int rowChoice = 0;
            int colChoice = 0;
            bool bFoundZero = false;
            for(int i = 0; i < dimensions; i++){
                for(int j = 0; j < dimensions; j++){
                    if(tiles[i,j] == 0){
                        bFoundZero = true;
                    }
                }
            }
            if(!bFoundZero){
                return false;
            }

            //this should be greatly improved before production - horrible performance as board fills up.
            while(!validChoice){
                Random random = new Random();
                rowChoice = random.Next(0, dimensions);
                colChoice = random.Next(0, dimensions);

                if(tiles[rowChoice,colChoice] == 0){
                    validChoice = true;
                }
            }
            PlayerMakeMove(rowChoice, colChoice,-1);
            return true;
        }

    #region Helpers
    //deserialize
        public int[,] Deserialize(string str){
            string temp = str.Replace("[","");
            string[] stringSeparators = new string[] {"],"};
            string[] result = temp.Split(stringSeparators, StringSplitOptions.None);
            //split first row for dimensions
            dimensions = result[0].Split(',').Count();
            result[dimensions-1] = result[dimensions-1].Replace("]","");
            tiles = new int[dimensions,dimensions];

            for(int i=0; i<dimensions; i++){
                string[] row = result[i].Split(',');
                for(int j = 0; j<dimensions; j++){
                    
                     tiles[i,j] = int.Parse(row.ElementAt(j).ToString());
                }
            }
            return tiles;
        }

        //function to validate legitimate moves
        private bool ValidMove(int row, int col){
            if(row <= dimensions && col <=  dimensions && tiles[row,col] == 0){
                return true;
            }
            return false;
        }

     public string CheckRows(){
            //sum rows
            bool bFoundZero = false;
            for(int i=0; i < dimensions; i++){
                int temp = 0;
                for(int j=0; j < dimensions; j++){
                    if(tiles[i,j] == 0){
                        bFoundZero = true;
                    }
                    temp += tiles[i,j];
                }
                if(temp == dimensions){
                    return "player";
                }else if(temp == -1*dimensions){
                    return "computer";
                }
            }

            if(bFoundZero){
                return "found zero";
            }
            return "";               
            }
        public string CheckCols(){
            //sum cols
            for(int i=0; i < dimensions; i++){
                int temp = 0;
                for(int j=0; j < dimensions; j++){
                    temp += tiles[j,i];
                }
                if(temp == dimensions){
                    return "player";
                }else if(temp == -1*dimensions){
                    return "computer";
                }
                
            }
            return "";
        }
        public string CheckDiag(){
            //sum diagonal
            for(int i =0; i < dimensions; i++){
                sumDiag += tiles[i,i];
            }
            if(sumDiag == dimensions){
                    return "player";
                }else if(sumDiag == -1*dimensions){
                    return "computer";
                }
                else{
                    return "";
                }
        }
        public string CheckDiag2(){
            
            int temp = 0;
            for (int i=0,j=dimensions-1 ; i<dimensions && j>=0 ; i++, j--) {
                  temp+= tiles[i,j];     
           }
            if(temp == dimensions){
                return "player";
            }else if(temp == -1*dimensions){
                return "computer";

            }
            return "";
        }
    #endregion
    }

}