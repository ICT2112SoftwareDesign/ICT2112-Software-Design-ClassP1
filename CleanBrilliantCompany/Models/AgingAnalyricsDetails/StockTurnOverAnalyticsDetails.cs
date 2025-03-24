using System;
using System.Collections.Generic;
using System.Linq;

public class StockTurnOverAnalyticsDetails : AbstractAnalyticsDetails {
    // 2 diff constructors
    // these are only init if its to generate a new dashboard
    private Dictionary<DateOnly, int> quantityPerDay = new(); // remainingStockPerday 
    private int totalQuantity; 

    // these will be retrieved from the database 
    private float turnOverRate = -1; 
    private float DeadStockPercentage =-1 ; 

    

    // constructor 
    public StockTurnOverAnalyticsDetails(int batchCode, int totalQuantity) : base(batchCode, "StockTurnOverAnalyticsDetails"){
        this.totalQuantity = totalQuantity; 
    } 
    public void setQuantityPerDay(Dictionary<DateOnly, int> quantityPerDay){
        this.quantityPerDay = quantityPerDay; 
    }   
    

    // constructor for retrieving from the database 
    public StockTurnOverAnalyticsDetails(int batchCode, float turnOverRate, float DeadStockPercentage) : base(batchCode, "StockTurnOverAnalyticsDetails"){
        this.turnOverRate = turnOverRate; 
        this.DeadStockPercentage = DeadStockPercentage; 
    } 

    // percentage of stock used
    public float calculateTurnOverRate(){
        // if quantityPerDay is empty, return 0 
        if (!quantityPerDay.Any()) return 0; 

        // get the first and last date 
        DateOnly firstDate = quantityPerDay.Keys.Min(); 
        DateOnly lastDate = quantityPerDay.Keys.Max(); 
        
        int earliestQuantity = quantityPerDay[firstDate]; 
        int latestQuantity = quantityPerDay[lastDate]; 
        int stockUsed = totalQuantity - latestQuantity; 

        return totalQuantity == 0 ? 0 : ((float)stockUsed / totalQuantity) * 100; 
    }    

    public float calculateDeadStockPercentage(){
        // if no quantityPerDay, return 100 
        if (!quantityPerDay.Any()) return 100; 
        DateOnly latestDate = quantityPerDay.Keys.Max(); 
        int latestQuantity = quantityPerDay[latestDate];
        return totalQuantity == 0 ? 0 : ((float)latestQuantity / totalQuantity) * 100; 

    } 

     
    public override Dictionary<string, object> CalculateBatchSummary(){
        Dictionary<string, object> batchSummary = new Dictionary<string, object>(); 
        // batchSummary.Add("TurnOverRate", calculateTurnOverRate()); 
        // batchSummary.Add("DeadStockPercentage", calculateDeadStockPercentage()); 
        //check if turnoverrate and deadstock are -1 
        if (turnOverRate == -1 && DeadStockPercentage == -1){
            //Console.WriteLine("Calculating TurnOverRate and DeadStockPercentage");
            batchSummary.Add("TurnOverRate", calculateTurnOverRate()); 
            batchSummary.Add("DeadStockPercentage", calculateDeadStockPercentage()); 
        } else {
            //Console.WriteLine("Retrieving TurnOverRate and DeadStockPercentage from the database");
            batchSummary.Add("TurnOverRate", turnOverRate); 
            batchSummary.Add("DeadStockPercentage", DeadStockPercentage); 
        }  

        return batchSummary; 
    } 

}
