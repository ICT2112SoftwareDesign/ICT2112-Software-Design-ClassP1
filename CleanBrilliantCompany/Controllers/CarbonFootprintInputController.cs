//using CleanBrilliantCompany.Domain;
//using CleanBrilliantCompany.Models;
//using CleanBrilliantCompany.DTO;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;

//namespace CleanBrilliantCompany.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CarbonFootprintInputController : ControllerBase
//    {
//        private readonly CarbonFootprintManagerControl _manager;
//        private readonly CarbonFootprintCalculatorControl _calculator;
//        private readonly CarbonFootprintRepositoryControl _repository;

//        public CarbonFootprintInputController(
//            CarbonFootprintManagerControl manager,
//            CarbonFootprintCalculatorControl calculator,
//            CarbonFootprintRepositoryControl repository
//        )
//        {
//            _manager = manager;
//            _calculator = calculator;
//            _repository = repository;
//        }

//        // Add a new carbon footprint record
//        [HttpPost("add")]
//        public IActionResult AddCarbonFootprint([FromBody] CarbonFootprintDTO dto)
//        {
//            if (dto == null)
//            {
//                return BadRequest("Invalid data.");
//            }

//            // Map DTO to RDM
//            var rdm = new CarbonFootprintRecordRDM(
//                0,
//                dto.EntityId,
//                dto.EntityType,
//                dto.CarbonEmission,
//                dto.EcoStatus,
//                dto.DateCreated
//            );

//            _manager.AddCarbonFootprintRecord(
//                rdm.getEntityIdForInsert(),
//                rdm.getEntityTypeForInsert(),
//                (float)rdm.getCarbonEmissionForCalculation(),
//                rdm.getEcoStatusForInsert(),
//                rdm.getDateCreatedForInsert().ToDateTime(TimeOnly.MinValue)
//            );

//            return Ok("Record added successfully.");
//        }

//        // Update an existing carbon footprint record
//        [HttpPut("update/{id}")]
//        public IActionResult UpdateCarbonFootprint(int id, [FromBody] CarbonFootprintDTO dto)
//        {
//            if (dto == null)
//            {
//                return BadRequest("Invalid data.");
//            }

//            // Map DTO to RDM
//            var rdm = new CarbonFootprintRecordRDM(
//                id,
//                dto.EntityId,
//                dto.EntityType,
//                dto.CarbonEmission,
//                dto.EcoStatus,
//                dto.DateCreated
//            );

//            _manager.UpdateCarbonFootprintRecord(
//                rdm.getCarbonFootprintIdForUpdate(),
//                rdm.getEntityIdForInsert(),
//                rdm.getEntityTypeForInsert(),
//                (float)rdm.getCarbonEmissionForCalculation(),
//                rdm.getEcoStatusForInsert()
//            );

//            return Ok("Carbon footprint record updated.");
//        }

//        // Delete a carbon footprint record
//        [HttpDelete("delete/{id}")]
//        public IActionResult DeleteCarbonFootprint(int id)
//        {
//            _manager.RemoveCarbonFootprintRecord(id);
//            return Ok("Carbon footprint record deleted.");
//        }

//        // Get all product carbon footprints
//        [HttpGet("products")]
//        public IActionResult GetProductCarbonFootprints()
//        {
//            var records = _calculator.getAllProductCarbonFootprint();

//            // Map to DTO
//            var result = records.Select(record => new CarbonFootprintDTO
//            {
//                EntityId = record.getEntityIdForInsert(),
//                EntityType = record.getEntityTypeForInsert(),
//                CarbonEmission = record.getCarbonEmissionForCalculation(),
//                EcoStatus = record.getEcoStatusForInsert(),
//                DateCreated = record.getDateCreatedForInsert()
//            }).ToList();

//            return Ok(result);
//        }

//        // Get all order carbon footprints
//        [HttpGet("orders")]
//        public IActionResult GetOrderCarbonFootprints()
//        {
//            var records = _calculator.getAllOrderCarbonFootprint();

//            // Map to DTO
//            var result = records.Select(record => new CarbonFootprintDTO
//            {
//                EntityId = record.getEntityIdForInsert(),
//                EntityType = record.getEntityTypeForInsert(),
//                CarbonEmission = record.getCarbonEmissionForCalculation(),
//                EcoStatus = record.getEcoStatusForInsert(),
//                DateCreated = record.getDateCreatedForInsert()
//            }).ToList();

//            return Ok(result);
//        }

//        // Get product + orders carbon footprint
//        [HttpGet("all")]
//        public IActionResult GetAllCarbonFootprints()
//        {
//            var productList = _calculator.getAllProductCarbonFootprint();
//            var orderList = _calculator.getAllOrderCarbonFootprint();

//            var combinedList = productList.Concat(orderList).ToList();

//            // Map to DTO
//            var result = combinedList.Select(record => new CarbonFootprintDTO
//            {
//                CarbonFootprintId = record.getCarbonFootprintIdForUpdate(),
//                EntityId = record.getEntityIdForInsert(),
//                EntityType = record.getEntityTypeForInsert(),
//                CarbonEmission = record.getCarbonEmissionForCalculation(),
//                EcoStatus = record.getEcoStatusForInsert(),
//                DateCreated = record.getDateCreatedForInsert()
//            }).ToList();

//            return Ok(result);
//        }

//        // Get total carbon footprint
//        [HttpGet("total")]
//        public IActionResult GetTotalCarbonFootprint()
//        {
//            var total = _calculator.getAllCarbonFootprint();
//            return Ok(new { totalCarbonFootprint = total });
//        }

//        // Check database status
//        [HttpGet("status")]
//        public IActionResult CheckDatabaseStatus()
//        {
//            bool status = _repository.getDatabaseQueryStatus();
//            return Ok(new { databaseStatus = status });
//        }
//    }
//}
