# UnityProject_Assassination

## 📌 프로젝트 개요

짧은 시간 내에 핵심 기능을 구현하면서,  
완성도보다는 **기획 및 시스템 구성 능력, 그리고 기능 간의 연결**에 초점을 두었습니다.

## 🎮 주요 구현 요소

### 🧠 적 AI 및 상태 머신
- 적은 시야 안의 플레이어를 탐지하고 추적합니다.
- `EnemyAI`, `EnemySight2D`, `EnemyPathfinder`로 기능 분리
- FSM 방식의 상태 전환 기반

### 🗡️ 암살 시스템
- 플레이어가 적의 뒤에서 접근 시 암살 가능
- `IAssassinatble`, `Player_Assassinated`, `EnemyAssassination` 등으로 구현

- ### 🧠 적 AI 및 상태 머신

- 적은 Patrol(순찰) 상태와 Chase(추적) 상태를 전환하며 플레이어를 추적합니다.
- `EnemyAI`와 `EnemySight2D`, `EnemyPathfinder` 등을 분리하여 구성하였고,
- `EnemyPathfinder`는 **Tilemap 기반 2D 경로 탐색 시스템**으로,
  **BFS (Breadth-First Search)** 알고리즘을 직접 구현하여 최단 경로를 계산합니다.

  ![image-1](https://github.com/user-attachments/assets/aabf06c0-15bb-4e78-82fa-090892390be1)
