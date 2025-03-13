using System;
using System.Collections.Generic;
using System.Linq;

public class StockTurnOverAnalyticsDetails : AbstractAnalyticsDetails {
    private Dictionary<DateTime, int> quantityPerDay; // remainingStockPerday 
    private int totalQuantity; 

    // constructor 
    public StockTurnOverAnalyticsDetails(int batchCode, Dictionary<DateTime, int> quantityPerDay, int totalQuantity) : base(batchCode, "StockTurnOverAnalyticsDetails"){
        this.quantityPerDay = quantityPerDay; 
        this.totalQuantity = totalQuantity; 
    } 

    // percentage of stock used
    public float calculateTurnOverRate(){
        // if quantityPerDay is empty, return 0 
        if (!quantityPerDay.Any()) return 0; 

        // get the first and last date 
        DateTime firstDate = quantityPerDay.Keys.Min(); 
        DateTime lastDate = quantityPerDay.Keys.Max(); 
        
        int earliestQuantity = quantityPerDay[firstDate]; 
        int latestQuantity = quantityPerDay[lastDate]; 

        int stockUsed = earliestQuantity - latestQuantity; 
        // if totalQuantity is 0 , return 0 
        // else return the percentage of stock used 
        return totalQuantity == 0 ? 0 : (stockUsed / totalQuantity) * 100; 
    }    

    public float calculateDeadStockPercentage(){
        if (!quantityPerDay.Any()) return 100; 
        DateTime latestDate = quantityPerDay.Keys.Max(); 
        int latestQuantity = quantityPerDay[latestDate];
        return totalQuantity == 0 ? 0 : (latestQuantity / totalQuantity) * 100; 

    } 

     
    public override Dictionary<string, object> CalculateBatchSummary(){
        Dictionary<string, object> batchSummary = new Dictionary<string, object>(); 
        batchSummary.Add("TurnOverRate", calculateTurnOverRate()); 
        batchSummary.Add("DeadStockPercentage", calculateDeadStockPercentage()); 
        return batchSummary; 
    } 

}
