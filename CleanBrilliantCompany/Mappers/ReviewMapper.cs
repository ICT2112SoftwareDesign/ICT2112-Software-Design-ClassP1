using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models; // <-- Add this


namespace CleanBrilliantCompany.Mappers
{
   public class ReviewMapper : IReviewDatabase
   { 
        private readonly string _connectionString; 
        private readonly IReviewQueryObserver _observer;


        public ReviewMapper(string connectionString, IReviewQueryObserver observer)
        { 
            _connectionString = connectionString;
            _observer = observer;
        }

        public bool addReview(int customerId, string reviewText, int rating, int productId)
        { 
            string sql = @"
                INSERT INTO Review (CustomerId, Review, Rating, ProductId)
                VALUES (@CustomerId, @Review, @Rating, @ProductId);
            "; 
            try
            { 
                using var connection = new SqlConnection(_connectionString); 
                using var command = new SqlCommand(sql, connection); 
                 command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@Review", reviewText);
                command.Parameters.AddWithValue("@Rating", rating);
                command.Parameters.AddWithValue("@ProductId", productId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                bool success = rowsAffected > 0;

                if (success)
                    _observer.OnReviewSubmitted(customerId, productId, rating);
                else
                    _observer.OnReviewQueryFailed(customerId, "Insert operation returned 0 rows affected.");

                return success;
            }
            catch (Exception ex)
            { 
                _observer.OnReviewQueryFailed(customerId, ex.Message);

                return false; 
            }

        }

        public bool updateReview(int customerId, string review, int rating, int reviewId)
        { 
            string sql = @"
                UPDATE Review
                SET review = @Review, Rating = @Rating
                WHERE ReviewId = @ReviewId AND CustomerId = @CustomerId;
            ";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@Review", review);
                command.Parameters.AddWithValue("@Rating", rating);
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                bool success = rowsAffected > 0;

                if (success)
                    _observer.OnReviewUpdated(reviewId, customerId, rating);
                else
                    _observer.OnReviewQueryFailed(customerId, "Update operation returned 0 rows affected.");

                return success;
            }
             catch (Exception ex)
            {
                _observer.OnReviewQueryFailed(customerId, ex.Message);
                return false;
            }

        }

         public bool deleteReview(int customerId, int reviewId)
        {
            string sql = @"
                DELETE FROM Review
                WHERE ReviewId = @ReviewId AND CustomerId = @CustomerId;
            ";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                bool success = rowsAffected > 0;

                if (success)
                    _observer.OnReviewDeleted(reviewId, customerId);
                else
                    _observer.OnReviewQueryFailed(customerId, "Delete operation returned 0 rows affected.");
                    
                return success;
            }
            catch (Exception ex)
            {
                _observer.OnReviewQueryFailed(customerId, ex.Message);

                return false;
            }
        }

        public List<ReviewRDM> GetAllReviews()
        {
            string sql = "SELECT ReviewId, CustomerId, review, Rating, ProductId FROM Review;";
            var reviews = new List<ReviewRDM>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var review = new ReviewRDM();
                    review.SetReviewId(Convert.ToInt32(reader["ReviewId"]));
                    review.SetCustomerId(Convert.ToInt32(reader["CustomerId"]));
                    review.SetReview(reader["review"].ToString());
                    review.SetRating(Convert.ToInt32(reader["Rating"]));
                    review.SetProductId(Convert.ToInt32(reader["ProductId"]));

                    reviews.Add(review);
                }
            }
            catch (Exception)
            {
                // TODO: log exception
            }

            return reviews;
        }



    } 
   
   }