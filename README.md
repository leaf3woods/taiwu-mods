# 太吾绘卷Mods
###### mods of the scroll of tai wu



##### 1.全优主角 / *straight_A_protagonist*

> Mod目录结构

* 游戏所在目录的Mod文件夹下：

```
└─straight_A_protagonist
    │  Config.lua
    │  girl.jpg
    │  Settings.Lua
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
