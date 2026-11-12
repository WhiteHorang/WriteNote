using System;

namespace WriteNote
{
    public class DocumentInfo
    {
        // 문서 제목
        public string Title { get; set; } = "";

        // 실제 RTF 파일명
        public string FileName { get; set; } = "";

        // 문서 생성 날짜
        public DateTime CreatedAt { get; set; }

        // 마지막 수정 날짜
        public DateTime ModifiedAt { get; set; }

        // 즐겨찾기 여부
        public bool IsFavorite { get; set; }
    }
}