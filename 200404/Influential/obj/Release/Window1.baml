   M S B A M L   `   `   `  ����   InfluenceLine ver1.0    Influential.Window1   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$$  静定梁影响线绘制系统��$��573.228��$��895.763�� � WindowState$ 	Maximized=�.��]� -   $��
dockPanel1��.+��� -   $��menuTabctrl��$
��Left=�$	��313��.@��� $��杆件绘制��$��16(�$��Microsoft YaHei)�.��� ���	#FFE5E5E5.+��� -   $��完成绘制��$��76,233,107,219q�$��Center=�$��Center=�$��20(�$��54��$	��120���� -   $��kaishiBt��$��开始绘制��$��76,108,107,344q�$��Center=�$��Center=�$��20(�$��54��$	��120���� $��支座添加��$��16(�.��� ���	#FFE5E5E5.+��� -   $��	gudzzRect��$��40��$��40��
 � Tag$ 1��$��44,61,219,405q�����  �� ImageSource$ Images/gudzz.png���� $$��请选择支座，拖拽添加��$
��Left=�$	��Top=�$��24(�$	��259��$��1,14,0,0q��� -   $��huodzz_Rect��$��40��$��132,61,131,405q�$��40��$ 2�� J� RenderTransformOrigin$ 1.6,-2.3&����� $ Images/huodzz.png���� -   $��zuogdd_Rect��$��40��$��28,148,235,318q�$��40��$ 3������ $ Images/zuogdd.png���� -   $��yougdd_Rect��$��40��$��148,148,115,318q�$��40��$ 4��$ 1.656,0.599&����� $ Images/yougdd.png���� -	   $��huadzzX_Rect��$��40��$��44,240,219,226q�$��40��$ 5������ $ Images/huadzzX.png���� -
   $��huadzzY_Rect��$��40��$��220,148,43,318q�$��40��$ 6��$ 0.441,-0.041&����� $ Images/huadzzY.png���� -   $��jiaolian_Rect��$��40��$��220,61,43,405q�$��40��$ 7��$ 0.441,-0.041&����� $ Images/jiaolian.png���� -   $��huadzzX2_Rect��$��40��$��132,240,131,226q�$��40��$ 8������ $ Images/huadzz-X.png���� -   $��huadzzY2_Rect��$��40��$��219,240,44,226q�$��40��$ 9��$ 0.441,-0.041&����� $ Images/huadzz-Y.png���� -   $��huodzz2_Rect��$��40��$��44,336,219,130q�$��40��$ 10��$ 1.6,-2.3&����� $ Images/huodzzX.png���� -   $��huodzz3_Rect��$��40��$��132,336,131,130q�$��40��$ 11��$ 1.6,-2.3&����� $ Images/huodzz-X.png���� -    ��Name$ text_zz1��$��
10,0,0,113q�$
��Left=�$��Bottom=� �� TextAlignment$ Center=�$��24(�当前选择：�� -   $ text_zz��$��148,0,0,113q�$
��Left=�$��Bottom=�$
 Left=�$ 0.722,-0.225&�$��24(�无�� -    ��Name$ bt_zyd��$��86,0,0,8q�$
��Left=�$��Bottom=�$��20(�$��52��$	��120��.��检查自由度:�� $��影响线绘制��$��16(�.��� ���	#FFE5E5E5.+��� -   $��InfluKindsTabctrl��$��0,0,0,0q�.@��� $��支座影响线��$��16(�.��� ���	#FFE5E5E5.+��� .@��� $��支座反力��.��� ���	#FFE5E5E5$��0,-4,0,4q�.+��� $��52,40,80,325q�$��Center=�$��Center=�$��24(�$��67��$	��151���� $�选择支座：���� -    ��Name$ 
zzComboBox��$��85,112,81,274q�$��Center=�$��Center=�	 �� SelectedValuePath$
	 zzID��
 �� DisplayMemberPath$
 zzName��$	��117��$��16(�$��46������ *	zzClass+ �� Mode$ OneWay=��� -   $ testBt��$��86,314,80,70q�$��Center=�$��Center=�$��重新绘制��$��16(�$��48��$	��117���� -   $��CheckAnswerBt��$��结果检查��$��Right=�$��Center=�$��0,208,82,177q�$��16(�$	��116��$��47���� -   $ CorrectAnswer��$��正确结果��$��Center=�$��Center=�$��86,260,81,123q�$��16(�$��49��$	��116���� $��支座反力偶��.��� ���	#FFE5E5E5.+��� $��44,35,89,331q�$��Center=�$��Center=�$��24(�$��66��$	��150���� $�选择支座：���� -   $ zzComboBox2��$��80,101,89,284q�$��Center=�$��Center=�$
	 zzID��$
 zzName��$	��114��$��16(�$��47������ *	zzClass+$ OneWay=��� -   $��FloBt��$��80,296,89,84q�$��Center=�$��Center=�$��重新绘制��$��16(�$��52��$	��114���� -   $��结果检查��$��Center=�$��Center=�$��80,194,89,192q�$��16(�$��46��$	��114���� -   $ CorrectAnswer2��$��正确结果��$��Center=�$��Center=�$��80,245,89,141q�$��16(�$��46��$	��114���� $��内力影响线��$��16(�.��� ���	#FFE5E5E5.+��� -   $��InternalForceTabctrl��.@��� $��弯矩��.��� ���	#FFE5E5E5.+��� -   $��WanJuBt��$
��Left=�$	��Top=�$��	10,53,0,0q�$��54��$	��112��.��选择截面�� -   $��WanJuResetBt��$
��Left=�$	��Top=�$��
150,53,0,0q�$��54��$	��108��.��重新绘制�� -   $��结果检查��$��
10,313,0,0q�$
��Left=�$	��Top=�$��51��$	��112���� -    $ CorrectAnswer4��$��正确结果��$��150,313,0,0q�$
��Left=�$	��Top=�$��51��$	��108���� $��
29,125,0,0q�$
��Wrap=�$��此截面所在杆：��$
��Left=�$	��Top=��� -!   $ CurrentSelectTB��$��
55,168,0,0q�$
��Left=�$	��Top=�
�������$	��100��$��36���� $��
29,216,0,0q�$
��Left=�$	��Top=�$
��Wrap=�$��距此杆左端距离：���� -"   $ CurrentSelectTB2��$��
55,252,0,0q�$
��Left=�$	��Top=�
�������$	��100��$��38���� $��剪力��.��� ���	#FFE5E5E5.+��� -#   $��JianLiBt��$
��Left=�$	��Top=�$��	10,52,0,0q�$��55��$	��112��.��选择截面�� -$   $��JianLiResetBt��$
��Left=�$	��Top=�$��
150,52,0,0q�$��55��$	��102��.��重新绘制�� -%   $��结果检查��$��
10,310,0,0q�$
��Left=�$	��Top=�$��45��$	��112���� -&   $ CorrectAnswer3��$��正确结果��$��150,310,0,0q�$
��Left=�$	��Top=�$��45��$	��102���� $��
29,125,0,0q�$
��Left=�$	��Top=�$
��Wrap=�$��此截面所在杆：���� -'   $ CurrentSelectTB3��$��
55,168,0,0q�$
��Left=�$	��Top=�
�������$	��100��$��36���� $��
29,216,0,0q�$
��Left=�$	��Top=�$
��Wrap=�$��距此杆左端距离：���� -(   $ CurrentSelectTB4��$��
55,252,0,0q�$
��Left=�$	��Top=�
�������$	��100��$��38���� -)   $��
qingkongBt��$��清空��$��Right=�$��75��$��Bottom=�$��18(��� -*   $��
textBlock1��$��Bottom=�$��Right=�$��80��$��25��$
��Wrap=�$��14(��� $	��Top=�$��24(�$
��Bold"�$ Center=�东北大学力学系�� -+   $	��can��$	��Top=����	#FFFBEFE3$ 0.5,0.5&� J� 	AllowDrop . z�V� .~���  �� ScaleY$ -1Q� �� ScaleX$ 1Q���  �� AngleY$ 0Q� �� AngleX$ 0Q���  �� Angle$ 0Q�T� 