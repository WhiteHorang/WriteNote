# WriteNote 테스트

## 1. 자동 통합 테스트 — `integration_test.ps1`

빌드한 `WriteNote.exe`를 화면 밖에서 실행하고, 코드에서 기능을 직접 호출해 확인한다. (108개 항목)

- 단축키, 글자 수, 붙여넣기, 저장·불러오기, 자동 저장, 테마, 글꼴, 문서 폴더·옮기기, 설정 유지, 설정 창,
  단축키 입력 창, 닫기 확인, 휴지통, 폴더별 분류, 맞춤법, 사용자 사전, 설명서, 문서 카드 우클릭 메뉴
- **실제 사용자 데이터(`%AppData%\WriteNote`)는 건드리지 않는다.** 환경 변수 `WRITENOTE_DATA_DIR`로
  `-work` 폴더 안의 테스트 전용 폴더를 쓰게 한다.
- 테스트 중에 뜨는 메시지 창은 자동으로 '취소/아니요'로 닫고 그 내용을 결과에 기록한다.

### 실행

```powershell
# 1) Debug 빌드 (Visual Studio 또는 MSBuild)
# 2) 저장소 폴더에서
powershell -STA -ExecutionPolicy Bypass -File tests\integration_test.ps1 -bin WriteNote\bin\Debug -work $env:TEMP\WriteNoteTest
```

- `-bin`: `WriteNote.exe`가 있는 폴더
- `-work`: 테스트용 문서와 데이터를 만들 임시 폴더 (실행할 때마다 안의 테스트 폴더를 지우고 다시 만든다)
- 결과: 화면 출력과 `-work\integration_progress.log` (통과/실패 | 항목 | 내용)

## 2. 한글 입력 확인 — `ime_typing_check.ps1`

자동 통합 테스트는 한글 입력기를 거치지 않으므로, 본문 입력과 관련된 기능을 바꿨을 때는 이 스크립트로
**실제 한글 입력기**를 거쳐 확인한다. (예: 천천히 칠 때 '됐다'가 'ㄷㅙㅆ다'로 끊기지 않는지)

> ⚠️ WriteNote 창을 띄워 실제 키 입력을 보낸다. 실행 중에는 키보드·마우스를 쓰지 않는다.
> 키를 보내기 전마다 WriteNote가 맨 앞 창인지 확인하고, 아니면 멈춘다.

```powershell
# 실제 데이터 대신 임시 폴더를 쓰게 한 뒤 실행
$env:WRITENOTE_DATA_DIR = "$env:TEMP\WriteNoteImeTest"
# 두벌식 키: e h o T e k = '됐다'. -delay는 키 사이 간격(ms)
powershell -ExecutionPolicy Bypass -File tests\ime_typing_check.ps1 -exe WriteNote\bin\Debug\WriteNote.exe -delay 600 -prefill 0 -keys "ehoTek goTek "
Remove-Item Env:\WRITENOTE_DATA_DIR
```

출력의 `본문 끝부분: [됐다 했다 ]`처럼 글자가 제대로 조합되어 있으면 정상이다.

## 3. 직접 확인할 항목

`docs\통합테스트_체크리스트.md` 4장 참고.
