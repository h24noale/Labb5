using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMI24H_VT25_SortSearch_Labb_
{
    public class LogEntry : IComparable<LogEntry>
    {
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int StatusCode { get; set; }

        public int CompareTo(LogEntry? other)
        {
            if (other == null) return 1;

            return Timestamp.CompareTo(other.Timestamp);
        }

        public override string ToString()
        {
            return $"{Timestamp} {IpAddress} {Method} {Path} {StatusCode}";
        }
    }
}