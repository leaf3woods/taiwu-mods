# 太吾绘卷Mods
###### mods of the scroll of tai wu



##### 1.全优主角 / *straight_A_protagonist*

> Mod目录结构

* 游戏文件夹的Mod下：

```
├─straight_A_protagonist
│  └─Plugins
│      ├─backup
│      └─Config
└─test
    └─Plugins
```

> ###### 说明

* 第一次成功调用此Mod后，将在dll所在目录下，以json格式生成配置文件以及游戏内所支持的所有特质列表文件（patch_settings.json & available_features.json）
* 第一次使用时，请前往available_features.json 文件中选择并拷贝你需要的特质至patch_settings.json 的CustomFeatures字段处，若未定义则使用默认池子
*  每个特质包含一个"IsLocked" 字段，如果你希望锁定某个特质，请找到对应特质并将"IsLocked"设置为true
* 本Mod将在玩家所选的池子内随机若干个特质，锁定的特质一定会出现

* 自定义特质数量通过patch_settings.json 中的"FeaturesCount"字段设置 默认为7个



