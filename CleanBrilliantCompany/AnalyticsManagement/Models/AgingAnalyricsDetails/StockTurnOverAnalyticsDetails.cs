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
 

    // constructor for retrieving from the database 
    public StockTurnOverAnalyticsDetails(int batchCode, float turnOverRate, float DeadStockPercentage) : base(batchCode, "StockTurnOverAnalyticsDetails"){
        this.turnOverRate = turnOverRate; 
        this.DeadStockPercentage = DeadStockPercentage; 
    } 
    public void SetQuantityPerDay(Dictionary<DateOnly, int> quantityPerDay){
        this.quantityPerDay = quantityPerDay; 
    }   

    // percentage of stock used
    public float CalculateTurnOverRate()
    {
        // Early returns for edge cases
        if (!quantityPerDay.Any() || totalQuantity == 0) return 0;
        
        // Get only needed quantity (we don't use firstDate/earliestQuantity)
        int latestQuantity = quantityPerDay[quantityPerDay.Keys.Max()];
        
        // Calculate and clamp in one step
        return Math.Clamp(
            ((float)(totalQuantity - latestQuantity) / totalQuantity) * 100, 
            0f, 
            100f
        );
    }

    public float CalculateDeadStockPercentage()
    {
        // Early returns for edge cases
        if (!quantityPerDay.Any()) return 100;
        if (totalQuantity == 0) return 0f;
        
        // Get the latest quantity
        int latestQuantity = quantityPerDay[quantityPerDay.Keys.Max()];
        
        // Calculate and clamp in one step
        return Math.Clamp(
            ((float)latestQuantity / totalQuantity) * 100,
            0f,
            100f
        );
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
