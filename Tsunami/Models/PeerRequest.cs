using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tsunami.Models
{
    public class PeerRequest
    {
        public int Length { get; set; }
        public int Piece { get; set; }
        public int Start { get; set; }

        public PeerRequest() { /* nothing to do. just for serializator */ }

        public PeerRequest(Core.PeerRequest pr)
        {
            Length = pr.length;
            Piece = pr.piece;
            Start = pr.start;
        }
    }
}
