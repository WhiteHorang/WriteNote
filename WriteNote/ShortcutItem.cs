using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WriteNote
{
    // 글쓰기 단축키 한 개의 정보
    public class ShortcutItem
    {
        // 수정·삭제할 때 항목을 구분하기 위한 고유 값
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        // 기능 이름 (예: 말줄임표 입력)
        public string Name { get; set; } = "";

        // 키 조합. JSON에는 "OemPeriod, Control"처럼 이름으로 저장한다.
        [JsonConverter(typeof(StringEnumConverter))]
        public Keys Keys { get; set; } = Keys.None;

        // 단축키를 누르면 본문에 입력할 내용
        public string Content { get; set; } = "";

        // 입력 후 커서 위치 (내용의 앞에서부터 몇 글자 뒤인지, -1이면 내용 끝)
        // 예: 「」 입력 후 커서를 괄호 안에 두려면 1
        public int CursorOffset { get; set; } = -1;

        // WriteNote가 기본으로 제공한 글쓰기 단축키인지 여부
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public string KeysText
        {
            get { return ShortcutKeyHelper.ToDisplayString(Keys); }
        }

        public ShortcutItem Clone()
        {
            return (ShortcutItem)MemberwiseClone();
        }
    }
}
