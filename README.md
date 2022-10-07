# 太吾绘卷Mods
###### mods of the scroll of tai wu

> GitHub开源： https://github.com/leaf3woods/tai_wu_mods

##### 1.全优主角 / *straight_A_protagonist*

> Mod目录结构（游戏所在目录）

* 初始：

```shell
└─straight_A_protagonist
    │  Config.lua
    │  local.jpg
    │  workshop.jpg
    │  README.md
    │  Settings.Lua
    │  setting_example.json
    │
    └─Plugins
            straight_A_protagonist.dll
            straight_A_protagonist.pdb
```

* 初次运行后

```shell
└─straight_A_protagonist
    │  Config.lua
    │  local.jpg
    │  workshop.jpg
    │  README.md
    │  Settings.Lua
    │  setting_example.json
    │
    └─Plugins
        │  straight_A_protagonist.dll
        │  straight_A_protagonist.pdb
        │
        └─Config
                available_basic_features.json
                available_basic_positive_features.json
                available_features.json
                patch_settings.json
```



> ###### 说明

* **setting_example.json 提供了配置文件的参考模板，使用优点特质池，特质数量7**
* 第一次成功加载此Mod后，将在dll 所在 Config/目录下，以json格式生成配置文件（patch_settings.json）以及当前游戏所支持的全特质池（available_features.json）、全基础特质池（"available_basic_features.json"）、全优点特质池（"available_basic_positive_features.json"）
* 第一次使用时，请前往所需的特质池中选择并拷贝你需要的特质至patch_settings.json 的CustomFeatures字段处，若未定义则使用默认池子
* 每个特质包含一个"IsLocked" 字段，如果你希望锁定某个特质，请在CustomFeatures中找到对应特质并将"IsLocked"字段设置为true
* 本Mod将在玩家所选的池子内随机若干个特质，锁定的特质一定会出现
* 自定义特质数量通过patch_settings.json 中的"FeaturesCount"字段设置 默认为7个
* “IfUnlockSameGroup”置true后将允许获得同组特质

> **注意**

* 本Mod 完全重写了获取基础特质获取的逻辑， 理论上获取的特质个数上限由你设置的自定义特质池子大小有关

* 但本Mod 默认会锁定同组特质只能获取一个，由于同组只能获取一个 ，所以最终生成的特质会比设置的少

* 设置的特质总数越多，完成角色初始特质越慢，目前限制最多尝试100次，就会结束获取特质的尝试，这也会导致特质比预先设置的少

> **其他**

* 日志通过后台程序输出，在logs文件夹下

> **例子**

* 以下这个例子选取了所有优点特质作为自定义特质池
* 同时解锁了同组特质只出现一次

```json
{
    "FeaturesCount": 7,
    "IfUseCustomFeaturePool": true,
    "IsOriginPoolGen": true,
    "IfUnlockSameGroup": true,
    "CustomFeatures": [
        {
            "Id": 0,
            "Name": "孔武有力",
            "GroupId": 0,
            "IsLocked": false
        },
        {
            "Id": 1,
            "Name": "牛虎怪力",
            "GroupId": 0,
            "IsLocked": false
        },
        {
            "Id": 2,
            "Name": "拔山扛鼎",
            "GroupId": 0,
            "IsLocked": false
        },
        {
            "Id": 6,
            "Name": "身强体健",
            "GroupId": 6,
            "IsLocked": false
        },
        {
            "Id": 7,
            "Name": "龙神马壮",
            "GroupId": 6,
            "IsLocked": false
        },
        {
            "Id": 8,
            "Name": "天人骨血",
            "GroupId": 6,
            "IsLocked": false
        },
        {
            "Id": 12,
            "Name": "耳聪目明",
            "GroupId": 12,
            "IsLocked": false
        },
        {
            "Id": 13,
            "Name": "剖决如流",
            "GroupId": 12,
            "IsLocked": false
        },
        {
            "Id": 14,
            "Name": "诡变无踪",
            "GroupId": 12,
            "IsLocked": false
        },
        {
            "Id": 18,
            "Name": "骨骼清奇",
            "GroupId": 18,
            "IsLocked": false
        },
        {
            "Id": 19,
            "Name": "形正神明",
            "GroupId": 18,
            "IsLocked": false
        },
        {
            "Id": 20,
            "Name": "仙胎仙骨",
            "GroupId": 18,
            "IsLocked": false
        },
        {
            "Id": 24,
            "Name": "才思敏捷",
            "GroupId": 24,
            "IsLocked": false
        },
        {
            "Id": 25,
            "Name": "灵心慧性",
            "GroupId": 24,
            "IsLocked": false
        },
        {
            "Id": 26,
            "Name": "智绝无双",
            "GroupId": 24,
            "IsLocked": false
        },
        {
            "Id": 30,
            "Name": "刚毅隐忍",
            "GroupId": 30,
            "IsLocked": false
        },
        {
            "Id": 31,
            "Name": "心清意明",
            "GroupId": 30,
            "IsLocked": false
        },
        {
            "Id": 32,
            "Name": "琨玉秋霜",
            "GroupId": 30,
            "IsLocked": false
        },
        {
            "Id": 36,
            "Name": "良才美玉",
            "GroupId": 36,
            "IsLocked": true
        },
        {
            "Id": 37,
            "Name": "不世奇才",
            "GroupId": 36,
            "IsLocked": true
        },
        {
            "Id": 38,
            "Name": "天元一气",
            "GroupId": 36,
            "IsLocked": true
        },
        {
            "Id": 42,
            "Name": "善解人意",
            "GroupId": 42,
            "IsLocked": false
        },
        {
            "Id": 43,
            "Name": "八面玲珑",
            "GroupId": 42,
            "IsLocked": false
        },
        {
            "Id": 44,
            "Name": "超凡脱俗",
            "GroupId": 42,
            "IsLocked": false
        },
        {
            "Id": 48,
            "Name": "福星高照",
            "GroupId": 48,
            "IsLocked": false
        },
        {
            "Id": 49,
            "Name": "禄马同乡",
            "GroupId": 48,
            "IsLocked": false
        },
        {
            "Id": 50,
            "Name": "如天至福",
            "GroupId": 48,
            "IsLocked": false
        },
        {
            "Id": 54,
            "Name": "皮坚骨硬",
            "GroupId": 54,
            "IsLocked": false
        },
        {
            "Id": 55,
            "Name": "钢筋铁骨",
            "GroupId": 54,
            "IsLocked": false
        },
        {
            "Id": 56,
            "Name": "金刚不坏",
            "GroupId": 54,
            "IsLocked": false
        },
        {
            "Id": 60,
            "Name": "六脉调和",
            "GroupId": 60,
            "IsLocked": false
        },
        {
            "Id": 61,
            "Name": "周天不息",
            "GroupId": 60,
            "IsLocked": false
        },
        {
            "Id": 62,
            "Name": "天人合一",
            "GroupId": 60,
            "IsLocked": false
        },
        {
            "Id": 66,
            "Name": "容光焕发",
            "GroupId": 66,
            "IsLocked": false
        },
        {
            "Id": 67,
            "Name": "生龙活虎",
            "GroupId": 66,
            "IsLocked": false
        },
        {
            "Id": 68,
            "Name": "百病不侵",
            "GroupId": 66,
            "IsLocked": false
        },
        {
            "Id": 72,
            "Name": "克己慎行",
            "GroupId": 72,
            "IsLocked": false
        },
        {
            "Id": 73,
            "Name": "沉稳果决",
            "GroupId": 72,
            "IsLocked": false
        },
        {
            "Id": 74,
            "Name": "稳如磐石",
            "GroupId": 72,
            "IsLocked": false
        },
        {
            "Id": 78,
            "Name": "三元汇聚",
            "GroupId": 78,
            "IsLocked": false
        },
        {
            "Id": 79,
            "Name": "先天气体",
            "GroupId": 78,
            "IsLocked": false
        },
        {
            "Id": 80,
            "Name": "身披华光",
            "GroupId": 78,
            "IsLocked": false
        },
        {
            "Id": 84,
            "Name": "意定神闲",
            "GroupId": 84,
            "IsLocked": false
        },
        {
            "Id": 85,
            "Name": "神融气泰",
            "GroupId": 84,
            "IsLocked": false
        },
        {
            "Id": 86,
            "Name": "八脉天通",
            "GroupId": 84,
            "IsLocked": false
        },
        {
            "Id": 90,
            "Name": "沉静寡言",
            "GroupId": 90,
            "IsLocked": false
        },
        {
            "Id": 91,
            "Name": "处变不惊",
            "GroupId": 90,
            "IsLocked": false
        },
        {
            "Id": 92,
            "Name": "优游自若",
            "GroupId": 90,
            "IsLocked": false
        },
        {
            "Id": 96,
            "Name": "思虑入微",
            "GroupId": 96,
            "IsLocked": false
        },
        {
            "Id": 97,
            "Name": "洞若观火",
            "GroupId": 96,
            "IsLocked": false
        },
        {
            "Id": 98,
            "Name": "火眼金睛",
            "GroupId": 96,
            "IsLocked": false
        },
        {
            "Id": 102,
            "Name": "眼疾手快",
            "GroupId": 102,
            "IsLocked": false
        },
        {
            "Id": 103,
            "Name": "听风辨位",
            "GroupId": 102,
            "IsLocked": false
        },
        {
            "Id": 104,
            "Name": "蹑影藏形",
            "GroupId": 102,
            "IsLocked": false
        },
        {
            "Id": 108,
            "Name": "胆大于身",
            "GroupId": 108,
            "IsLocked": false
        },
        {
            "Id": 109,
            "Name": "无所畏惧",
            "GroupId": 108,
            "IsLocked": false
        },
        {
            "Id": 110,
            "Name": "舍生忘死",
            "GroupId": 108,
            "IsLocked": false
        },
        {
            "Id": 114,
            "Name": "临机应变",
            "GroupId": 114,
            "IsLocked": false
        },
        {
            "Id": 115,
            "Name": "沉机观变",
            "GroupId": 114,
            "IsLocked": false
        },
        {
            "Id": 116,
            "Name": "穷极要妙",
            "GroupId": 114,
            "IsLocked": false
        },
        {
            "Id": 120,
            "Name": "动如脱兔",
            "GroupId": 120,
            "IsLocked": false
        },
        {
            "Id": 121,
            "Name": "电光石火",
            "GroupId": 120,
            "IsLocked": false
        },
        {
            "Id": 122,
            "Name": "奔逸绝尘",
            "GroupId": 120,
            "IsLocked": false
        },
        {
            "Id": 126,
            "Name": "杀气腾腾",
            "GroupId": 126,
            "IsLocked": false
        },
        {
            "Id": 127,
            "Name": "心无恻隐",
            "GroupId": 126,
            "IsLocked": false
        },
        {
            "Id": 128,
            "Name": "恶鬼罗刹",
            "GroupId": 126,
            "IsLocked": false
        },
        {
            "Id": 132,
            "Name": "脉象殷实",
            "GroupId": 132,
            "IsLocked": false
        },
        {
            "Id": 133,
            "Name": "气海充盈",
            "GroupId": 132,
            "IsLocked": false
        },
        {
            "Id": 134,
            "Name": "真气精纯",
            "GroupId": 132,
            "IsLocked": false
        },
        {
            "Id": 138,
            "Name": "五行俱全",
            "GroupId": 138,
            "IsLocked": false
        },
        {
            "Id": 139,
            "Name": "太和灵气",
            "GroupId": 138,
            "IsLocked": false
        },
        {
            "Id": 140,
            "Name": "混元奇窍",
            "GroupId": 138,
            "IsLocked": false
        },
        {
            "Id": 144,
            "Name": "健步如飞",
            "GroupId": 144,
            "IsLocked": false
        },
        {
            "Id": 145,
            "Name": "高视阔步",
            "GroupId": 144,
            "IsLocked": false
        },
        {
            "Id": 146,
            "Name": "夭矫如龙",
            "GroupId": 144,
            "IsLocked": false
        },
        {
            "Id": 150,
            "Name": "体质特异",
            "GroupId": 150,
            "IsLocked": false
        },
        {
            "Id": 151,
            "Name": "冰心玉骨",
            "GroupId": 150,
            "IsLocked": false
        },
        {
            "Id": 152,
            "Name": "百毒不侵",
            "GroupId": 150,
            "IsLocked": false
        },
        {
            "Id": 156,
            "Name": "手长脚长",
            "GroupId": 156,
            "IsLocked": true
        },
        {
            "Id": 157,
            "Name": "亭亭鹤立",
            "GroupId": 156,
            "IsLocked": true
        },
        {
            "Id": 158,
            "Name": "蜂腰猿背",
            "GroupId": 156,
            "IsLocked": false
        },
        {
            "Id": 162,
            "Name": "情有独钟",
            "GroupId": 162,
            "IsLocked": false
        },
        {
            "Id": 163,
            "Name": "从一而终",
            "GroupId": 162,
            "IsLocked": false
        },
        {
            "Id": 164,
            "Name": "忠贞不渝",
            "GroupId": 162,
            "IsLocked": false
        }
    ]
}
```

