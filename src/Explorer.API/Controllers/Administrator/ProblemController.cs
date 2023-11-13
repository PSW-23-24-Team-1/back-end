using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administrator/problem")]
    public class ProblemController : BaseApiController
    {
        private readonly IProblemService _problemService;
        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        [HttpGet]
        public ActionResult<PagedResult<ProblemResponseDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _problemService.GetAll(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPut("set-deadline/{problemId:long}")]
        public ActionResult<ProblemResponseDto> SetDeadline([FromBody] ProblemDeadlineUpdateDto problem)
        {
            var result = _problemService.UpdateDeadline(problem.Id, problem.Deadline);
            return CreateResponse(result);
        }

        [HttpGet("{problemId:long}/resolve")]
        public ActionResult ResolveProblem(long problemId)
        {
            var problem = _problemService.Get(problemId).Value;
            if (!problem.IsResolved && problem.IsAnswered)
            {
                var result = _problemService.ResolveProblem(problemId);
                return CreateResponse(result);
            }
            return Forbid();
        }

        [HttpGet("{problemId:long}/problem-answer")]
        public ActionResult<ProblemAnswerDto> GetProblemAnswer(long problemId)
        {
            var result = _problemService.GetAnswer(problemId);
            return CreateResponse(result);
        }
    }
}
