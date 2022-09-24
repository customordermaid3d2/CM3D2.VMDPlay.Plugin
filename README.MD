# 이게 뭐지

단순하게 베핀용으로 바꾼거  
그리고 취향에 맞게 개조중  

# Change Log : 0.3.11.2
 - COM3D2.VMDPlay.Plugin 업데이트
   - 어깨, 팔상하박 XYZ 회전 QuickAdjust 추가
   - Favorite 에 QuickAdjust 파라미터 저장/불러오기 포함
     - 총 10개 파라미터 : 어째, 상박, 하박 XYZ, 모델 scale
   - Load 시에 윈도우 숨김 기능 제거


# 개조한거

- 즐겨찿기 기능 추가
- 모션 타임 시프트 기능 추가
- 필요없는 기능 제거
- UI 내맘대로 초기화
- 최적화 시도중 (계속 실패중이지만)



# 샘플 영상  

https://youtu.be/0nGCOD7KlPg  
https://youtu.be/LHRlHhz0dEM  


# 필요한거

- BepInEx https://github.com/BepInEx/BepInEx  
- COM3D2.API.dll  https://github.com/DeathWeasel1337/COM3D2_Plugins/releases/download/v3/COM3D2.API.v1.0.zip  
- UnityEngine.dll (5.6.1)
- Assembly-CSharp.dll (COM3D2\COM3D2x64_Data\Managed)
- Assembly-CSharp-firstpass.dll (COM3D2\COM3D2x64_Data\Managed)

# 참고

- 처음 실행시 COM3D2\BepInEx\config 폴더에아래 두개 파일 생성됨  
COM3D2.VMDPlay.Plugin.cfg : 설정 파일   
COM3D2.VMDPlay.Plugin.json : 즐겨찿기 파일    

- 파일 경로는 전부 상대경로 처리  
  드라이브가 다를경우 테스트 안해봄  
