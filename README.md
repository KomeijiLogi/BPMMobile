# BPMMobile
### This is the dealing approval lignt app which based on the YUN app
### 这是基于云之家的流程审批轻应用
###### updatelog:
##### 更新日志:
==============================================================================
###### 流程审批之前的流程暂时没有时间记录，从2017/7/6开始记录本轻应用的更新情况
==============================================================================

###### 2017/7/6
###### 目前部署了如下流程：
###### 洁瑞通用流程
###### 洁瑞物料申请
###### 洁瑞样品申请
###### 周工作汇报
###### BG0702001 商管公司内部请示
###### BG0702002 商管公司部门联系
###### BG0702008 商管公司图纸会审
###### BG0702009 商管公司装修押金退还审批
###### BG0702012 商管公司开业保证金、履约保证金退还审批
###### CW0701008 房地产集团其他费用申请(通用)
###### CW0701009 房地产集团其他费用报销(通用)
###### CW0701011 房地产集团推广费用申请
###### FLSW0702001 盛祥商管公司招商合同评审
###### FLSW0702002 盛祥商管公司其他合同评审

###### 添加发起，审批,拒绝，退回重填，加签，知会，重新发起功能
==========================================================================

###### 2017/7/6
###### 追加退回某步功能
==========================================================================

###### 2017/7/8
###### 添加流程:
###### SCGL0703002 建筑公司不良物资提报
###### CGGL0703002 建设公司安质部零星采购申请
==========================================================================

###### 2017/7/12
###### 添加流程:
###### 商管公司周工作汇报
###### 商管公司月工作汇报
===========================================================================

###### 2017/7/13
###### 添加流程:
###### BG0702005 盛祥商管公司合同评审
###### 对所有流程的已阅操作追加选填提交信息
===========================================================================

###### 2017/7/14
###### 添加流程:
###### BG0702004 盛祥商管公司威高广场代金劵、礼品领用申请
###### BG0702007 盛祥商管公司工装制作申请
===========================================================================

###### 2017/7/25
###### 添加市场差旅费报销流程（预装），添加差旅费的城市列表功能
===========================================================================

###### 2017/7/26
###### 添加费用特批（预装）
###### 添加流量统计模块
###### 添加版本控制模块
===========================================================================

###### 2017/7/29
###### 添加其他费用报销（预装）
###### 添加外贸公司计划评审流程
###### 对差旅费的城市选择模块进行改版，添加历史城市查询功能，添加草稿功能，
###### 并对差旅费部分细节进行改良，优化调整部分组件的布局
===========================================================================


###### 2017/8/5
###### BPMDATA数据库下建立dbo.GetBPMOU存储过程，用来根据组织code来获取相关组织，应用于建设公司通知公告流程选取组织中
###### PSDATA数据库下建立dbo.erpcloud_getPerInfo存储过程，用来根据工号查询PS中人员的相关信息
###### PSDATA数据库下建立dbo.erpcloud_getPerInfoByArr存储过程,用来根据工号数组查询PS中人员的相关信息
=============================================================================================================


###### 2017/8/8
###### 添加流程：
###### SC0200002   富森公司外贸订单评审
###### RLZY0106002 建设公司行管（销售）人员试用期转正申请
###### ZLGL0106001 建设公司车间生产过程检验记录备案
###### 98JSGZZD    建设公司规章制度申请
###### 98JSTZGG    建设公司通知公告申请
###### 98JSHYJY    建设公司会议纪要申请
###### RLZY0106004 建设公司车间员工转正参保申请
###### BG0702003   盛祥商管公司威高广场消费卡领用申请
###### JJGC0106001 建设公司工程签证备案申请
===================================================================================================================

###### 2017/8/10
###### 修复了审批过程中审核同意时审批意见没发送到下一审批节点的bug
###### 修复了正式环境下待办信息处理完没有转变成已办的bug
###### 修复了明细列表项中必填字段的校验失效的bug，增添处理逻辑
###### 优化了项目中文件的存放结构，现在根据组织架构找流程更方便了
###### 优化了在showPicker的处理逻辑，添加showPickerOptimize方法，防止创建冗余picker
###### 优化了差旅费等流程中添加子项的处理逻辑，将展示子项放到添加按钮之上，现在点击添加会自动定位到添加按钮
==============================================================================================================================


###### 2017/8/10
###### 合并流程

###### BG0702006 盛祥商管公司办公设备维修、更换申请
###### BG0702011 盛祥商管公司商户撤场申请

###### XZBG0106001 建设公司证件借用申请
###### RLZY0106001 建设公司车间员工请假申请
###### RLZY0106003 建设公司行管（销售）人员转正参保申请
###### AQGL0106001 建设公司安全检查问题反馈
###### ZLGL0106003 建设公司生产质量问题反馈
###### CWGL0106001 建设公司月度材料采购付款计划申请
###### SCYX0703001 建设公司门窗售后维修采购申请
###### ZLGL0703002 建设公司产成品出厂检验记录备案

================================================================================================================================
 