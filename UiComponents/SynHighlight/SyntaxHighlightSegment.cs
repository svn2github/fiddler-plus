using System;
using System.Collections.Generic;

namespace tevton.SyntaxHighlight {
    public class SyntaxHighlightSegment {
        private int start;
        private int end;
        public int Start {
            get { return start; }
            set { start = value; }
        }
        public int End {
            get { return end; }
            set { end = value; }
        }
        public int OrderedStart {
            get { return Math.Min(Start, End); }
        }
        public int OrderedEnd {
            get { return Math.Max(Start, End); }
        }
        public int Length {
            get { return OrderedEnd - OrderedStart; }
        }
        public SyntaxHighlightSegment(int start, int end){
            this.Start = start;
            this.End = end;
        }
        public virtual bool Contains(SyntaxHighlightSegment segment) {
            return this.OrderedStart <= segment.OrderedStart &&
                this.OrderedEnd >= segment.OrderedEnd;
        }
        public virtual bool Overlaps(SyntaxHighlightSegment segment) {
            return (this.OrderedStart <= segment.OrderedStart &&
                    this.OrderedEnd > segment.OrderedStart) ||
                (this.OrderedStart < segment.OrderedEnd &&
                this.OrderedEnd >= segment.OrderedEnd);
            //return (this.OrderedStart <= segment.OrderedStart && 
            //        this.OrderedEnd >= segment.OrderedStart) ||
            //    (this.OrderedStart <= segment.OrderedEnd && 
            //    this.OrderedEnd >= segment.OrderedEnd);
        }
        /// <summary>
        /// A Segment is SuperiorTo another segment when it completely contains another segment
        /// or overlaps it and starts earlier.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public virtual bool SuperiorTo(SyntaxHighlightSegment segment) {
            return this.Contains(segment) || (this.Overlaps(segment) && this.OrderedStart < segment.OrderedStart);
        }
    }
}