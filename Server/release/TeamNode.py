# -*- coding: gbk -*-

class   TeamNode:   
          """分组二叉树节点"""   
          def   __init__(self,name=None,lchild=None,rchild=None,level=None,levelno=None,pLevel=None,pNo=None,data=None):   
                  self.name=name #节点名称
                  self.lchild=lchild   #节点左孩子
                  self.rchild=rchild   #节点右孩子
                  self.level=level     #当前等级 例如:4
                  self.levelno=levelno #当前序号
                  self.pLevel=pLevel #父节点等级 例如:A
                  self.parentNo=pNo  #父节点序号
                  self.data=data   #节点数据
                  
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
                  print '节点:',node.name,'数据:',node.data,'父节点:',node.pLevel,node.pNo,'当前:',node.level,node.levelno 
                  self.preorder(node.lchild)   
                  self.preorder(node.rchild)   

          def   searchCommonFather1(self,node,count=1):   
                  if   node==None:return   
                  return   node
                  self.preorder(node.lchild,count)   
                  self.preorder(node.rchild,count)   
                  count = count + 1






 
