# -*- coding: gbk -*-

class   TeamNode:   
          """����������ڵ�"""   
          def   __init__(self,name=None,lchild=None,rchild=None,level=None,levelno=None,pLevel=None,pNo=None,data=None):   
                  self.name=name #�ڵ�����
                  self.lchild=lchild   #�ڵ�����
                  self.rchild=rchild   #�ڵ��Һ���
                  self.level=level     #��ǰ�ȼ� ����:4
                  self.levelno=levelno #��ǰ���
                  self.pLevel=pLevel #���ڵ�ȼ� ����:A
                  self.parentNo=pNo  #���ڵ����
                  self.data=data   #�ڵ�����
                  
class   BinaryTree:
          """   

0                                  A0                              
                      /                        \              
1                    B0                        B1                                   
                 /       \           /                 \      
2              C0        C1         C2                 C3                
             /   \     /    \      /       \        /      \                      
3           D0   D1    D2    D3    D4      D5      D6      D7        
           /\   / \   / \   / \   / \     /  \    /  \    /  \             
4        E0 E1 E2 E3 E4 E5 E6 E7 E8 E9 E10 E11 E12 E13 E14 E15        
                                                                      
        """     
          def   __init__(self,root):   
                  self.root=root    
    
          def   preorder(self,node):   
                  if   node==None:return   
                  print '�ڵ�:',node.name,'����:',node.data,'���ڵ�:',node.pLevel,node.pNo,'��ǰ:',node.level,node.levelno 
                  self.preorder(node.lchild)   
                  self.preorder(node.rchild)   

          def   searchCommonFather1(self,node,count=1):   
                  if   node==None:return   
                  return   node
                  self.preorder(node.lchild,count)   
                  self.preorder(node.rchild,count)   
                  count = count + 1






 
