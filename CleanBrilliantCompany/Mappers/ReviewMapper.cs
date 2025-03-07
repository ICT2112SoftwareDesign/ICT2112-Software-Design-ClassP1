using System;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Mappers
{
   public class ReviewMapper : iReviewDatabase
   { 
        private readonly string _connectionString; 

        public ReviewMapper(string connectionString)
        { 
            _connectionString = connectionString;
        }

        public bool addReview(int customerId, string review, int rating)
        { 
            //logic
            return false; 
        }

        public bool updateReview(int customerId, string review, int rating)
        { 
            //logic
            return false; 
        }

        public bool deleteReview(int customerId, int reviewId)
        { 
            //logic
            return false; 
        }

         public bool notifyDBCustomerQueryStatus()
        {
            // Implementation logic here
            return false;
        }


   }
}