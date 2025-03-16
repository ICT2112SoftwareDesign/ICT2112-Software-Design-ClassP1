using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarbonFootprintInputController : ControllerBase
    {
        private readonly CarbonFootprintManagerControl _manager;
        private readonly CarbonFootprintCalculatorControl _calculator;
        private readonly CarbonFootprintRepositoryControl _repository;

        public CarbonFootprintInputController(
            CarbonFootprintManagerControl manager,
            CarbonFootprintCalculatorControl calculator,
            CarbonFootprintRepositoryControl repository
        )
        {
            _manager = manager;
            _calculator = calculator;
            _repository = repository;
        }

        // Add a new carbon footprint record
        [HttpPost("add")]
        public IActionResult AddCarbonFootprint([FromBody] CarbonFootprintRecordRDM record)
        {
            _manager.AddCarbonFootprintRecord(
                record.getEntityIdForInsert(),
                record.getEntityTypeForInsert(),
                (float)record.getCarbonEmissionForCalculation(),
                record.getEcoStatusForInsert(),
                record.getDateCreatedForInsert().ToDateTime(TimeOnly.MinValue)
            );

            return Ok("Carbon footprint record added.");
        }

        // Update an existing carbon footprint record
        [HttpPut("update/{id}")]
        public IActionResult UpdateCarbonFootprint(int id, [FromBody] CarbonFootprintRecordRDM record)
        {
            _manager.UpdateCarbonFootprintRecord(
                id,
                record.getEntityIdForInsert(),
                record.getEntityTypeForInsert(),
                (float)record.getCarbonEmissionForCalculation(),
                record.getEcoStatusForInsert()
            );

            return Ok("Carbon footprint record updated.");
        }

        // Delete a carbon footprint record
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCarbonFootprint(int id)
        {
            _manager.RemoveCarbonFootprintRecord(id);
            return Ok("Carbon footprint record deleted.");
        }

        // Get all product carbon footprints
        [HttpGet("products")]
        public IActionResult GetProductCarbonFootprints()
        {
            var records = _calculator.getAllProductCarbonFootprint();

            // Map to DTO
            var result = records.Select(record => new CarbonFootprintDTO
            {
                EntityId = record.getEntityIdForInsert(),
                EntityType = record.getEntityTypeForInsert(),
                CarbonEmission = record.getCarbonEmissionForCalculation(),
                EcoStatus = record.getEcoStatusForInsert(),
                DateCreated = record.getDateCreatedForInsert()
            }).ToList();

            return Ok(result);
        }

        // Get all order carbon footprints
        [HttpGet("orders")]
        public IActionResult GetOrderCarbonFootprints()
        {
            var records = _calculator.getAllOrderCarbonFootprint();

            // Map to DTO
            var result = records.Select(record => new CarbonFootprintDTO
            {
                EntityId = record.getEntityIdForInsert(),
                EntityType = record.getEntityTypeForInsert(),
                CarbonEmission = record.getCarbonEmissionForCalculation(),
                EcoStatus = record.getEcoStatusForInsert(),
                DateCreated = record.getDateCreatedForInsert()
            }).ToList();

            return Ok(result);
        }

        // Get product + orders carbon footprint
        [HttpGet("all")]
        public IActionResult GetAllCarbonFootprints()
        {
            var productList = _calculator.getAllProductCarbonFootprint();
            var orderList = _calculator.getAllOrderCarbonFootprint();

            var combinedList = productList.Concat(orderList).ToList();

            // Map to DTO
            var result = combinedList.Select(record => new CarbonFootprintDTO
            {
                EntityId = record.getEntityIdForInsert(),
                EntityType = record.getEntityTypeForInsert(),
                CarbonEmission = record.getCarbonEmissionForCalculation(),
                EcoStatus = record.getEcoStatusForInsert(),
                DateCreated = record.getDateCreatedForInsert()
            }).ToList();

            return Ok(result);
        }

        // Get total carbon footprint
        [HttpGet("total")]
        public IActionResult GetTotalCarbonFootprint()
        {
            var total = _calculator.getAllCarbonFootprint();
            return Ok(new { totalCarbonFootprint = total });
        }

        // Check database status
        [HttpGet("status")]
        public IActionResult CheckDatabaseStatus()
        {
            bool status = _repository.getDatabaseQueryStatus();
            return Ok(new { databaseStatus = status });
        }
    }
}
