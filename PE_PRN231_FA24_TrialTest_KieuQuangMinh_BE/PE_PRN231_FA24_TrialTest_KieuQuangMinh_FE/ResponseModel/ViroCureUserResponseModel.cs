using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ResponseModel
{
	public class ViroCureUserResponseModel
	{
		public int Id { get; set; }
		public string Email { get; set; } = null!;
		public string Role { get; set; }
	}
}