using System.Collections.Generic;

namespace tevton.SyntaxHighlight {
    public class SyntaxHighlightSegmentList: List<SyntaxHighlightSegment> {
        /// <summary>
        /// Remove segments from this list that are overlapped by those in masterList
        /// </summary>
        /// <param name="masterList"></param>
        public virtual void RemoveOverlappingSegments(SyntaxHighlightSegmentList masterList) {
            SyntaxHighlightSegmentList res = new SyntaxHighlightSegmentList();
            foreach(SyntaxHighlightSegment thisSegment in this) {
                bool doAdd = true;
                foreach(SyntaxHighlightSegment masterSegment in masterList) {
                    if(masterSegment.SuperiorTo(thisSegment)) {
                        doAdd = false;
                        break;
                    }
                }
                if(doAdd) {
                    res.Add(thisSegment);
                }
            }
            this.Clear();
            this.AddRange(res);
        }
        /// <summary>
        /// Remove segments from this list that are overlapped by another segments in the same list
        /// </summary>
        public virtual void RemoveOverlappingSegments() {
            SyntaxHighlightSegmentList res = new SyntaxHighlightSegmentList();
            res.AddRange(this);
            for(int i = res.Count - 1; i >= 0; i--) {
                for(int j = 0; j < i; j++) {
                    if(this[j].SuperiorTo(res[i])) {
                        res.RemoveAt(i);
                        break;
                    }
                }
            }
            this.Clear();
            this.AddRange(res);
        }
    }
}
