using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Aspose.Net.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AsposeController : ControllerBase
    {
        
        public IActionResult GetDocument()
        {
            Document document = new Document(@"wwwroot\AsposeDocument.docx");
            DocumentBuilder builder = new DocumentBuilder(document);

            builder.Writeln("Initial test document");
            

            return Ok(new { success = true });
        }
       
    }
}
