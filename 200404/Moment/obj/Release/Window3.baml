   M S B A M L   `   `   `  ����   WpfApplication3    WpfApplication3.Window3   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$'  !静定结构弯矩图绘制系统��$��722.641��$��862.687�� � WindowState$ 	Maximized=�.��]� -   $��
dockPanel1��.+��� -   $��menuTabctrl��$
��Left=�$	��200��$
��1200��$	��Top=�.@��� $��杆件��$��30��$��18(�$��95��$��0,0,2,0q�.��� ���	#FFE5E5E5 � ColumnDefinitions �� $
��143* ��� $	��48* �.+��� -   $��完成绘制��$
��Left=�$��
35,237,0,0q�$	��Top=�$	��120��$��40��$��2���� -   $��kaishiBt��$��开始绘制杆件 ��$
��Left=�$��	35,60,0,0q�$	��Top=�$	��120��$��40��$��2���� -   $��help1��$��帮助��$
��Left=�$��40��$��
35,349,0,0q�$	��Top=�$	��120��$��2���� $��比例尺：��$
��Left=�$��32��$��
18,135,0,0q�$	��Top=�$��81���� -   $	��blc��$
��Left=�$��32��$��
35,171,0,0q� �� TextWrapping$
 Wrap=�$	��1:1��$	��Top=�$	��120��$��20(�$��2�� �� TextAlignment$ Center=��� $��支座��$��18(�$��-2,0,5,0q�$��30��$��95��$��Bottom=�.��� ���	#FFE5E5E5$��-1,0,1,0q�.+��� -   $��
gudzz_Rect��$
��Left=�$��40��$	��Top=�$��40�� J� RenderTransformOrigin$ -4.967,-1.133&�
 � Tag$ 1��$��	38,30,0,0q�����  �� ImageSource$1 +/WpfApplication3;component/Images/gudzz.png���� $$��请选择支座，拖拽添加��$
��Left=�$	��Top=��� -   $��huodzz_Rect��$
��Left=�$��40��$��
117,30,0,0q�$	��Top=�$��40��$ 2������ $3 -/WpfApplication3;component/Images/zhizuoY.png���� -	   $��zuogdd_Rect��$
��Left=�$��40��$��	20,95,0,0q�$	��Top=�$��40��$ 3������ $2 ,/WpfApplication3;component/Images/zuogdd.png���� -
   $��yougdd_Rect��$
��Left=�$��40��$��
117,95,0,0q�$	��Top=�$��40��$ 4��$ 1.656,0.599&����� $2 ,/WpfApplication3;component/Images/yougdd.png���� -   $��huadzzX_Rect��$
��Left=�$��40��$��
38,175,0,0q�$	��Top=�$��40��$ 5������ $3 -/WpfApplication3;component/Images/huadzzX.png���� -   $��huadzzY_Rect��$
��Left=�$��40��$��117,175,0,0q�$	��Top=�$��40��$ 6������ $3 -/WpfApplication3;component/Images/huadzzY.png���� -   $��jiaolian_Rect��$
��Left=�$��40��$��
38,240,0,0q�$	��Top=�$��40��$ 7��$ 0.441,-0.041&����� $4 ./WpfApplication3;component/Images/jiaolian.png���� -    ��Name$ text_zz1��$��0,297,20,253q�当前选择：�� -   $ text_zz��$��88,297,0,253q�0�� -   $��zz_ok��$��完成约束��$��
0,390,35,0q�$	��Top=�$��40��$��Right=�$	��120���� -   $��huodongx_Rect��$
��Left=�$��40��$��117,240,0,0q�$	��Top=�$ 8��$��40������ $3 -/WpfApplication3;component/Images/zhizuoX.png���� -   $��help2��$��帮助��$��Right=�$��40��$��
0,480,50,0q�$	��Top=�$��90���� $��载荷��$��18(�$
��Left=�$��30��$	��Top=�$��95��$��0q�.��� ���	#FFE5E5E5$��0,0,2,0q�	 � RowDefinitions	 �� $
��0.2* ��� $��30 ��� $
��0.3* ��� $��50 ��� $
��0.8* ��� $��50 ��� $��50 �.+�� $
��10 5q�$��0��.+��� -   $��shangjzl_Rect��$
��Left=�$��50��$��10,3,0,0q�$	��Top=�$��10��$ 9������ $4 ./WpfApplication3;component/Images/shangjzl.png���� -   $��youjzl_Rect��$
��Left=�$��10��$��	60,10,0,0q�$	��Top=�$��50��$ 11������ $2 ,/WpfApplication3;component/Images/youjzl.png���� -   $��zuojzl_Rect��$
��Left=�$��10��$��	60,40,0,0q�$	��Top=�$��50��$ 12������ $2 ,/WpfApplication3;component/Images/zuojzl.png���� -   $��xiajzl_Rect��$
��Left=�$��50��$��	150,3,0,0q�$	��Top=�$��10��$ 10������ $2 ,/WpfApplication3;component/Images/xiajzl.png���� 
 �� Orientation$
 
Horizontal=�$��Center=�$��22,61,28,30q�.+��� $��大小F:��$
��Left=�$��40��$��0,0,0,0q�$	��Top=�$��62���� -    ��Name$ 	textbox_F��|�. $
��Left=�$��40��$��0,0,0,0q�$
 Wrap=�$	��Top=�$��55���� $��1��
���   �$��Bottom=�$��
1,0,-1,-36q��� $��1��
���   �$��Bottom=�$��1,0,-1,-260q��� -    ��Name$ btn_F_ok��$��48,106,40,-20q�$��确认输入���� $��1��$
 
Horizontal=�$��Center=�� $
��10,0q�$��2��.+��� -   $��youjbzh_Rect��$��50��$��10,25,140,0q�$	��Top=�$ 13������ $3 -/WpfApplication3;component/Images/youjbzh.png���� -   $��xiajbzh_Rect��$��20��$��
63,25,57,0q�$	��Top=�$ 15������ $3 -/WpfApplication3;component/Images/xiajbzh.png���� -   $��shangjbzh_Rect��$
��Left=�$��20��$��	63,55,0,0q�$	��Top=�$��50��$ 16������ $5 //WpfApplication3;component/Images/shangjbzh.png���� -   $��zuojbzh_Rect��$
��Left=�$��50��$��
140,25,0,0q�$	��Top=�$��20��$ 14������ $3 -/WpfApplication3;component/Images/zuojbzh.png���� $
 
Horizontal=�$��Center=�$��	0,80,0,56q�.+��� $��	长度 l:��$
��Left=�$��40��$��0,0,0,0q�$	��Top=�$��64���� -   $ 	textbox_l��$
��Left=�$��40��$
 Wrap=�$	��Top=�$��65���� $
 
Horizontal=�$��Center=�$��20,122,20,0q�.+��� $��大小q:��$
��Left=�$��50��$��0,0,0,0q�$	��Top=�$��64���� -   $ 	textbox_q��$
��Left=�$��40��$
 Wrap=�$	��Top=�$��65���� $��2��$
 
Horizontal=�$��Center=�$��44,166,44,10q�$��2��.+��� -   $ btn_q_ok��$��80��$��
10,1,10,45q�$��确认输入��� $��10 10q�$��4��.+��� $
 
Horizontal=�$��
0,-40,0,40q�.+��� -     ��Name$ shunljX_Rect��$
��Left=�$��40��$
��10,0q�$	��Top=�$��30��$ 17��$ 
0.567,-0.7&����� $3 -/WpfApplication3;component/Images/shunljX.png���� -!   $ 
niljX_Rect��$
��Left=�$��40��$��0,0,10,0q�$	��Top=�$��30��$ 18������ $1 +/WpfApplication3;component/Images/niljX.png���� -"   $ shunljY_Rect��$
��Left=�$��40��$��	0,0,10,57q�$	��Top=�$��30��$ 19������ $3 -/WpfApplication3;component/Images/shunljY.png���� -#   $ 
niljY_Rect��$
��Left=�$��40��$��0,0,10,0q�$	��Top=�$��30��$ 20������ $1 +/WpfApplication3;component/Images/niljY.png���� $
 
Horizontal=�$��Center=�$	��Top=�$��0,10,0,0q�.+��� $��大小M:��$
��Left=�$��40��$��71���� -$   $ 	textbox_M��$
��Left=�$��40��$
 Wrap=�$��68���� $��4��$
 
Horizontal=�$��Center=�$��44,17,44,418q�.+��� -%   $ btn_M_ok��$��40��$��80��$
��10,5q�$��确认输入���� $��4��$
 
Horizontal=�$��Center=�$��-12,115,0,400q�.+��� -&   $ help3��$��40��$��80��$��10,14q�.��帮助�� -'   $ btn_sure��$��40��$��80��$
��10,0q�.��完成�� $��练习��$��18(�$
��Left=�$��30��$	��Top=�$��95��$��-2,0,0,0q�.��� ���	#FFE5E5E5.+��� -(   $��practice��$��练习��$
��Left=�$��40��$��	35,20,0,0q�$	��Top=�$	��120���� -)   $��chose1��$��杆件选取��$
��Left=�$��40��$��	35,90,0,0q�$	��Top=�$	��120���� -*   $��caotu1��$��区段叠加��$
��Left=�$��40��$��
35,160,0,0q�$	��Top=�$	��120���� -+   $��draw1_ok��$��弯矩图绘制��$
��Left=�$��40��$��
35,230,0,0q�$	��Top=�$	��120���� -,   $��	reaction1��$��支座反力提示��$
��Left=�$	�� 40��$	��Top=�$��
35,300,0,0q�$	��120���� --   $��score1��$��打分��$
��Left=�$��40��$��
35,370,0,0q�$	��Top=�$	��120���� -.   $��bending��$��参考答案��$
��Left=�$��40��$��
35,440,0,0q�$	��Top=�$	��120���� -/   $��help4��$��帮助��$
��Left=�$��
35,510,0,0q�$	��120��$��40��$	��Top=��� -0   $ 
textBlock1��$��20(� �� TextAlignment$ Center=�$	��Top=�$-��'结构力学弯矩图绘制学习系统��$
��Wrap=�$��34���� -1   $	��can��$	��Top=����	#FFFBEFE3|�. $ 0.5,0.5&� J� 	AllowDrop . $��-7,0,0,0q�z�V� .~���  �� ScaleY$ -1Q� �� ScaleX$ 1Q���  �� AngleY$ 0Q� �� AngleX$ 0Q���  �� Angle$ 0Q�T� 