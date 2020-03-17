# RPGAutoChess

## 设计思路

### 逻辑

#### Manager

用于承载各个模块的主要逻辑

- ChessManager 管理所有棋子，在这里触发所有棋子的更新
- MapManager 管理所有地块（地块种类这种功能还未实现）
- IOManager 处理用户输入，输出变更信息（维护事件队列）

虽然有些不符合“承载模块逻辑”的定义，但是也仍作为一个Manager子类

- TimerTools 给各个模块提供定时回调功能，需要每次Update被调用

#### Event

用于解耦上层逻辑与基础业务

- Event 具体事件（例如攻击事件，携带攻击者、被攻击者、攻击伤害等信息）
- EventHandler 可以处理某种Event，并需要向dispatcher注册
- EventDispatcher 负责分发事件给对应的Handler
- Manager 当**即将**发生某种变更时，向dispatcher抛出事件，等待事件处理完毕后应用数据

#### Buff & Skill

### 前后台通信

#### Message

用作后台向前台传输数据

- Complete 全量信息，例如ChessCompleteMessage就传输了所有棋子数组
- Increment 增量信息，例如ChessAttachIncrementMessage就传输了攻击者被攻击者、伤害，需要接受到消息后修改本地状态，才能完成与服务器同步

**Message与Event的同一性，准备后面完成**

#### Request

将本地操作传输给后台

未完待续...



