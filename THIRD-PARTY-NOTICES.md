# 외부 구성 요소 고지 (Third-Party Notices)

WriteNote는 아래 외부 구성 요소를 **수정하지 않고 그대로** 사용합니다.
배포용 `WriteNote.exe`에는 이 구성 요소들이 리소스로 들어 있으며, 각 라이선스 전문은
프로그램 설명서(F1) 13장과 이 저장소의 `WriteNote/Licenses/`, `WriteNote/Dictionaries/` 폴더에 있습니다.

---

## 1. hunspell-dict-ko 0.7.94 — 한국어 맞춤법 사전

| 항목 | 내용 |
|---|---|
| 사용 파일 | `ko.aff`, `ko.dic` (맞춤법 검사에 쓰는 사전 데이터) |
| 라이선스 | **GPL-3.0** (사전을 만든 원본 데이터는 MPL-1.1/GPL-2.0/LGPL-2.1, CC-BY-SA-4.0 등을 포함하며, 합쳐서 만든 사전 파일은 GPL-3.0) |
| 저작권 | Copyright (C) 2008-2019 hunspell-dict-ko contributors, Copyright (C) 2017-2019 National Institute of Korean Language(국립국어원) |
| 수정 여부 | 수정하지 않음 (원작자 Release의 `ko-aff-dic-0.7.94.zip`에 들어 있는 파일 그대로) |
| 원본 소스 (Corresponding Source) | 원작자 저장소의 `0.7.94` 태그: https://github.com/spellcheck-ko/hunspell-dict-ko/tree/0.7.94 (커밋 `07c3392`) |
| 원작자 Release | https://github.com/spellcheck-ko/hunspell-dict-ko/releases/tag/0.7.94 |
| 라이선스 문서 | `WriteNote/Dictionaries/LICENSE-ko-dictionary.md`, `WriteNote/Dictionaries/LICENSE.GPL-3.txt` |

- 사전은 WriteNote가 실행할 때 **데이터로 읽어 쓰는 별개의 저작물**입니다.
  사전 라이선스 문서의 *Note about GPL Compatibility*에 따라, 사전을 사용하는 프로그램은 GPL-3.0과 라이선스가 맞지 않아도 됩니다.
- 사전 자체는 GPL-3.0에 따라 누구나 복사·수정·재배포할 수 있습니다. 위 원본 소스 링크에서 사전을 만드는 원본 데이터와 도구를 받을 수 있습니다.

## 2. WeCantSpell.Hunspell 7.0.1 — 맞춤법 검사 라이브러리

| 항목 | 내용 |
|---|---|
| 사용 파일 | `WeCantSpell.Hunspell.dll` (NuGet 패키지 `WeCantSpell.Hunspell` 7.0.1의 `lib/netstandard2.0`) |
| 라이선스 | MPL 1.1 / GPL 2.0 / LGPL 2.1 중 선택 (원본 Hunspell에서 물려받음). WriteNote는 **MPL 1.1** 조건으로 사용합니다. |
| 수정 여부 | 수정하지 않음 |
| 원본 소스 | 원작자 저장소의 `7.0.1` 태그: https://github.com/aarondandy/WeCantSpell.Hunspell/tree/7.0.1 (NuGet 패키지에 기록된 원본 커밋 `ab5709d95b2d23541984d22baa0ab2d1e783582f`) |
| 라이선스 문서 | `WriteNote/Licenses/WeCantSpell.Hunspell-LICENSE.txt` |

## 3. Newtonsoft.Json 13.0.4 — JSON 저장·불러오기

| 항목 | 내용 |
|---|---|
| 라이선스 | MIT — Copyright (c) 2007 James Newton-King |
| 수정 여부 | 수정하지 않음 |
| 출처 | https://github.com/JamesNK/Newtonsoft.Json |
| 라이선스 문서 | `WriteNote/Licenses/Newtonsoft.Json-LICENSE.md` |

## 4. Microsoft .NET 보조 구성 요소

`System.Memory` 4.5.4, `System.Buffers` 4.5.1, `System.Numerics.Vectors` 4.5.0, `System.Runtime.CompilerServices.Unsafe` 6.0.0
(WeCantSpell.Hunspell이 .NET Framework에서 동작하는 데 필요)

| 항목 | 내용 |
|---|---|
| 라이선스 | MIT — Copyright (c) .NET Foundation and Contributors |
| 수정 여부 | 수정하지 않음 |
| 출처 | https://github.com/dotnet/corefx (4.5.x), https://github.com/dotnet/runtime (Unsafe 6.0.0) |
| 라이선스 문서 | `WriteNote/Licenses/Microsoft-dotnet-LICENSE.txt` |

---

이 고지는 구성 요소의 라이선스 조건(라이선스 전문 포함, 원본 소스 안내)을 지키기 위한 것입니다.
버전을 바꾸면 이 문서의 버전과 링크도 함께 바꿔야 합니다.
