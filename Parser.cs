using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{

    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        static bool bool_assign = true;
        static bool bool_funCall = true;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        //hadeeeeeeeeeeeel
        Node Program()
        {
            Node program = new Node("Program Function");
            //code cfg Rule
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.MainFunction != TokenStream[InputPointer + 1].token_type)
                {
                    program.Children.Add(function_sate_dash());
                }
                program.Children.Add(mainFun());

            }

            MessageBox.Show("Success");
            return program;
        }
        Node function_sate_dash()
        {
            Node func = new Node("function_sate");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.DataType_Int == TokenStream[InputPointer].token_type || Token_Class.DataType_String == TokenStream[InputPointer].token_type
                    || Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {
                    if (Token_Class.MainFunction != TokenStream[InputPointer + 1].token_type)
                    {
                        func.Children.Add(Func_stat());
                        func.Children.Add(function_sate_dash());
                        return func;
                    }
                }

            }
            return null;

        }
        Node mainFun()
        {
            Node mainfunc = new Node("mainfunc");
            mainfunc.Children.Add(match(Token_Class.DataType_Int));
            mainfunc.Children.Add(match(Token_Class.MainFunction));
            mainfunc.Children.Add(match(Token_Class.LParanthesis));
            mainfunc.Children.Add(match(Token_Class.RParanthesis));
            mainfunc.Children.Add(func_body());
            return mainfunc;

        }
        Node ConditionState()
        {
            Node conditionState = new Node("ConditionState");
            conditionState.Children.Add(Condition());
            conditionState.Children.Add(ConditionStateDash());
            return conditionState;
        }
        Node ConditionStateDash()
        {
            Node conditionState = new Node("ConditionStateD");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.And == TokenStream[InputPointer].token_type || Token_Class.Or == TokenStream[InputPointer].token_type)
                {
                    conditionState.Children.Add(BoolOpp());
                    conditionState.Children.Add(ConditionState());
                }
                else
                    return null;
            }

            return conditionState;
        }
        Node BoolOpp()
        {
            Node opp = new Node("bool");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.And == TokenStream[InputPointer].token_type)
                {
                    opp.Children.Add(match(Token_Class.And));
                }
                else if (Token_Class.Or == TokenStream[InputPointer].token_type)
                {
                    opp.Children.Add(match(Token_Class.Or));
                }
                
            }
            return opp;
        }
        Node dataType()
        {
            Node data = new Node("data type");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {
                    data.Children.Add(match(Token_Class.DataType_Float));
                    //     return data;
                }
                else if (Token_Class.DataType_Int == TokenStream[InputPointer].token_type)
                {
                    data.Children.Add(match(Token_Class.DataType_Int));
                    //      return data;
                }
                else if (Token_Class.DataType_String == TokenStream[InputPointer].token_type)
                {
                    data.Children.Add(match(Token_Class.DataType_String));
                    // return data;
                }
                // else
                //   return null;
            }
            //  return null;
            return data;

        }
        Node Factor()
        {
            Node fac = new Node("factor");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Endl_T == TokenStream[InputPointer].token_type)
                {
                    fac.Children.Add(match(Token_Class.Endl_T));

                    return fac;
                }
                else
                {
                    fac.Children.Add(Expression());

                    return fac;
                }

            }

            return null;
        }
        Node readS()
        {
            Node read = new Node("read");
            read.Children.Add(match(Token_Class.Read));
            read.Children.Add(match(Token_Class.Identifier));
            read.Children.Add(match(Token_Class.Semicolon));
            return read;
        }
        Node writeS()
        {
            Node wr = new Node("write");

            wr.Children.Add(match(Token_Class.Write));
            wr.Children.Add(Factor());
            wr.Children.Add(match(Token_Class.Semicolon));

            return wr;
        }
        Node returnS()
        {
            Node read = new Node("return");
            read.Children.Add(match(Token_Class.Return));
            read.Children.Add(Expression());
            read.Children.Add(match(Token_Class.Semicolon));
            //read.Children.Add(Expression());
            return read;
        }
        Node repeatS()
        {
            Node Repeat = new Node("Repeat statement");
            Repeat.Children.Add(match(Token_Class.Repeat));
            Repeat.Children.Add(Statments());
            Repeat.Children.Add(match(Token_Class.Until));
            Repeat.Children.Add(ConditionState());
            return Repeat;
        }

        Node Condition()
        {
            Node condition = new Node("condition");
            condition.Children.Add(match(Token_Class.Identifier));
            condition.Children.Add(Operator());
            condition.Children.Add(Term());
            return condition;
        }

        Node Operator()
        {
            Node opperator = new Node("opperator");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.GreaterThanOp == TokenStream[InputPointer].token_type)
                {
                    opperator.Children.Add(match(Token_Class.GreaterThanOp));
                    return opperator;
                }
                else if (Token_Class.LessThanOp == TokenStream[InputPointer].token_type)
                {
                    opperator.Children.Add(match(Token_Class.LessThanOp));
                    return opperator;
                }
                else if (Token_Class.NotEqualOp == TokenStream[InputPointer].token_type)
                {
                    opperator.Children.Add(match(Token_Class.NotEqualOp));
                    return opperator;
                }
                else if (Token_Class.Equal == TokenStream[InputPointer].token_type)
                {
                    opperator.Children.Add(match(Token_Class.Equal));
                    return opperator;
                }
            }
            return null;
        }
        //noooran


        Node Func_stat()
        {
            Node func_stat = new Node("FunctionStatement");

            func_stat.Children.Add(func_dec());//Func_Decl
            func_stat.Children.Add(func_body());//Func_Body
            return func_stat;
        }

        Node If_statement()
        {
            Node if_statement = new Node("IfStatement");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    if_statement.Children.Add(match(Token_Class.If));
                    if_statement.Children.Add(ConditionState());
                    if_statement.Children.Add(match(Token_Class.Then));
                    if_statement.Children.Add(Statments());//statements
                    if_statement.Children.Add(ElseClause());
                    return if_statement;
                }
            }
            return null;
        }

        Node ElseClause()
        {
            Node elseClause = new Node("elseClause");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.end == TokenStream[InputPointer].token_type)
                {
                    elseClause.Children.Add(match(Token_Class.end));
                }
                else if (Token_Class.Elseif == TokenStream[InputPointer].token_type)
                {
                    elseClause.Children.Add(Elseif_stat());
                }
                else if (Token_Class.Else == TokenStream[InputPointer].token_type)
                {
                    elseClause.Children.Add(Else_stat());
                }
            }
            return null;

        }

        Node Elseif_stat()
        {
            Node elseif_stat = new Node("elseif_stat");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Elseif == TokenStream[InputPointer].token_type)
                {
                    elseif_stat.Children.Add(match(Token_Class.Elseif));
                    elseif_stat.Children.Add(ConditionState());
                    elseif_stat.Children.Add(match(Token_Class.Then));
                    elseif_stat.Children.Add(Statments());//statements
                    elseif_stat.Children.Add(ElseClause());
                    return elseif_stat;
                }
            }
            return null;
        }

        Node Else_stat()
        {
            Node else_stat = new Node("else_stat");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Else == TokenStream[InputPointer].token_type)
                {
                    else_stat.Children.Add(match(Token_Class.Else));
                    else_stat.Children.Add(Statments());//statements
                    else_stat.Children.Add(match(Token_Class.end));
                    return else_stat;
                }
            }
            return null;
        }


        /////// Declaration_state
        Node Declaration_state()
        {
            Node dec_state = new Node("dec_state");
            dec_state.Children.Add(dataType());
            dec_state.Children.Add(Declaration_state1());
            dec_state.Children.Add(Declaration_state2());
            dec_state.Children.Add(match(Token_Class.Semicolon));


            return dec_state;
        }
        Node Declaration_state1()
        {
            Node dec_state1 = new Node("dec_state1");
            if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
            {
                if (Token_Class.Assign == TokenStream[InputPointer + 1].token_type)
                {
                    bool_assign = false;
                    dec_state1.Children.Add(Assignment_state());
                }
                else
                    dec_state1.Children.Add(match(Token_Class.Identifier));
            }
            //   else
            //       dec_state1.Children.Add(Assignment_state());

            return dec_state1;
        }

        Node Declaration_state2()
        {
            Node dec_state2 = new Node("dec_state2");
            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                dec_state2.Children.Add(match(Token_Class.Comma));
                if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
                {
                    if (Token_Class.Assign == TokenStream[InputPointer + 1].token_type)
                    {
                        bool_assign = false;
                        dec_state2.Children.Add(Assignment_state());
                    }
                    else
                        dec_state2.Children.Add(match(Token_Class.Identifier));
                }
                dec_state2.Children.Add(Declaration_state2());

            }
            else
                return null;

            return dec_state2;
        }

        ////// Assignment_state
        Node Assignment_state()
        {
            Node Assign_state = new Node("Assign_state");

            Assign_state.Children.Add(match(Token_Class.Identifier));
            Assign_state.Children.Add(match(Token_Class.Assign));
            Assign_state.Children.Add(Expression());
            if (bool_assign == true)
            {
                Assign_state.Children.Add(match(Token_Class.Semicolon));
            }
            else
            {
                bool_assign = true;
            }

            return Assign_state;
        }

        ///// term 
        Node Term()
        {
            Node term = new Node("term");
            if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
            {
                if (Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type)
                {
                    bool_funCall = false;
                    term.Children.Add(Function_call());
                }
                else
                    term.Children.Add(match(Token_Class.Identifier));
            }
            else if (Token_Class.Number == TokenStream[InputPointer].token_type)
                term.Children.Add(match(Token_Class.Number));
            //  else
            //     term.Children.Add(Function_call());

            return term;
        }
        //// expression
        Node Expression()
        {
            Node expression = new Node("expression");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.String == TokenStream[InputPointer].token_type)
                    expression.Children.Add(match(Token_Class.String));
                else if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                    expression.Children.Add(Equation());
                else if (Token_Class.Number == TokenStream[InputPointer].token_type)
                {
                    if (Token_Class.DivideOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.MinusOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.MultiplyOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.PlusOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type
                         )
                        expression.Children.Add(Equation());
                    else
                        expression.Children.Add(Term());
                }
                
                else if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
                {
                    if (Token_Class.LParanthesis != TokenStream[InputPointer + 1].token_type)//term
                    {
                        if (Token_Class.DivideOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.MinusOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.MultiplyOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.PlusOp == TokenStream[InputPointer + 1].token_type
                         || Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type
                         )
                            expression.Children.Add(Equation());
                        else
                            expression.Children.Add(Term());

                    }
                    else //fun call
                    {
                        int x = InputPointer + 1;
                        while (TokenStream[x].token_type != Token_Class.RParanthesis && x < TokenStream.Count)
                        {
                            x++;
                        }
                        //x++;
                        if (Token_Class.RParanthesis == TokenStream[x].token_type)
                        {
                            if (Token_Class.DivideOp == TokenStream[x + 1].token_type
                             || Token_Class.MinusOp == TokenStream[x + 1].token_type
                             || Token_Class.MultiplyOp == TokenStream[x + 1].token_type
                             || Token_Class.PlusOp == TokenStream[x + 1].token_type
                             || Token_Class.LParanthesis == TokenStream[x + 1].token_type
                             )
                                expression.Children.Add(Equation());
                            else
                                expression.Children.Add(Term());
                        }
                    }
                }

            }
            return expression;

        }

        ////// equation
       
        Node Equation()
        {
            Node eqq1 = new Node("Equation");
            if (InputPointer < TokenStream.Count)
            {

                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    eqq1.Children.Add(match(Token_Class.LParanthesis));
                    eqq1.Children.Add(Term());
                    eqq1.Children.Add(EquationDash());
                    eqq1.Children.Add(match(Token_Class.RParanthesis));
                    if (Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                                   Token_Class.MultiplyOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.PlusOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                    {
                        eqq1.Children.Add(Arith_op());
                        eqq1.Children.Add(Equation());
                    }
                }
                else
                {
                    eqq1.Children.Add(Term());
                    eqq1.Children.Add(EquationDash());
                }
            }

            return eqq1;
        }

        Node EquationDash()
        {
            Node eqq = new Node("EquationDash");
            if (InputPointer < TokenStream.Count)
            {
                eqq.Children.Add(Arith_op());
                eqq.Children.Add(EquatDash());

            }

            return eqq;
        }
        Node EquatDash()
        {
            Node eqq = new Node("EquatDash");
            if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
            {
                eqq.Children.Add(match(Token_Class.LParanthesis));
                eqq.Children.Add(Term());
                eqq.Children.Add(EquationDash());
                eqq.Children.Add(match(Token_Class.RParanthesis));
                if (Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                                   Token_Class.MultiplyOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.PlusOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                {
                 
                    eqq.Children.Add(Arith_op());
                    eqq.Children.Add(EquatDash());
                }
            }

            else
            {
                eqq.Children.Add(Term());
                eqq.Children.Add(EquatD());
            }
            return eqq;
        }

        Node EquatD()
        {
            Node eqq = new Node("EquatD");
            if (Token_Class.MinusOp == TokenStream[InputPointer].token_type ||
                                   Token_Class.MultiplyOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.PlusOp == TokenStream[InputPointer].token_type ||
                                    Token_Class.DivideOp == TokenStream[InputPointer].token_type)

            {
                eqq.Children.Add(EquationDash());
                return eqq;

            }
            else
                return null;
        }
        Node Arith_op()
        {
            Node opp = new Node("Arith_op");
            if (Token_Class.MinusOp == TokenStream[InputPointer].token_type)
                opp.Children.Add(match(Token_Class.MinusOp));
            else if (Token_Class.MultiplyOp == TokenStream[InputPointer].token_type)
                opp.Children.Add(match(Token_Class.MultiplyOp));
            else if (Token_Class.PlusOp == TokenStream[InputPointer].token_type)
                opp.Children.Add(match(Token_Class.PlusOp));
            else if (Token_Class.DivideOp == TokenStream[InputPointer].token_type)
                opp.Children.Add(match(Token_Class.DivideOp));
            else
                return null;
            return opp;
        }
            ////// function call 
            Node Function_call()
        {
            Node fun_call = new Node("fun_call");

            fun_call.Children.Add(match(Token_Class.Identifier));
            fun_call.Children.Add(match(Token_Class.LParanthesis));
            fun_call.Children.Add(Parameters1());
            fun_call.Children.Add(match(Token_Class.RParanthesis));
            if (bool_funCall)
                fun_call.Children.Add(match(Token_Class.Semicolon));
            else
                bool_funCall = true;
            return fun_call;
        }

        Node Parameters1()
        {
            Node param1 = new Node("param1");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
                {
                    param1.Children.Add(match(Token_Class.Identifier));
                    param1.Children.Add(Parameters2());
                }
                else if (Token_Class.Number == TokenStream[InputPointer].token_type)
                {
                    param1.Children.Add(match(Token_Class.Number));
                    param1.Children.Add(Parameters2());
                }
                else
                    return null;
            }
            return param1;
        }
        Node Parameters2()
        {
            Node param2 = new Node("param2");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    param2.Children.Add(match(Token_Class.Comma));
                    if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
                    {
                        param2.Children.Add(match(Token_Class.Identifier));
                        param2.Children.Add(Parameters2());
                    }
                    else if (Token_Class.Number == TokenStream[InputPointer].token_type)
                    {
                        param2.Children.Add(match(Token_Class.Number));
                        param2.Children.Add(Parameters2());
                    }
                    param2.Children.Add(Parameters2());
                }
                else
                    return null;
            }
            return param2;
        }
  
        Node func_name()
        {   //cheack function name

            Node func_name = new Node("name");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Identifier == TokenStream[InputPointer].token_type)
                {
                    func_name.Children.Add(match(Token_Class.Identifier));
                    return func_name;
                }
                else
                    return null;
            }


            return null;


        }


        Node prameter()  //prameter ==> data type identifier
        {
            Node prameter = new Node("prameter");
            if (InputPointer < TokenStream.Count)
            {
                if (
                    Token_Class.DataType_Int == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_String == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {

                    prameter.Children.Add(dataType());
                    prameter.Children.Add(match(Token_Class.Identifier));

                }
                else
                    return null;
            }
            else
                return null;


            return prameter;


        }

        Node pr_()
        {
            Node pr = new Node("pr");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    pr.Children.Add(match(Token_Class.Comma));
                    pr.Children.Add(prameter());
                    pr.Children.Add(pr_());
                }
                else
                    return null;
            }
            else
                return null;

            return pr;

        }

        Node pram()
        {


            Node pram = new Node("pram");



            pram.Children.Add(prameter());
            pram.Children.Add(pr_());



            return pram;


        }
        Node func_dec_pram()////(pram|empty)
        {
            Node decpram = new Node("decpram");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {

                    decpram.Children.Add(match(Token_Class.LParanthesis));

                    decpram.Children.Add(pram());
                    decpram.Children.Add(match(Token_Class.RParanthesis));
                    //decpram.Children.Add(match(Token_Class.Semicolon));
                }
                else
                    return null;
            }
            else
                return null;



            return decpram;
        }



        Node func_dec()
        {
            Node dec = new Node("dec");
            if (InputPointer < TokenStream.Count)
            {
                if (
                    Token_Class.DataType_Int == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_String == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {



                    dec.Children.Add(dataType());
                    dec.Children.Add(func_name());
                    dec.Children.Add(func_dec_pram()); //(pram|empty)


                }

                else
                    return null;
            }

            else
                return null;
            return dec;



        }

        Node statement()
        {
            Node state = new Node("state");


            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comment == TokenStream[InputPointer].token_type)
                {

                    state.Children.Add(match(Token_Class.Comment));
                }
                else if (Token_Class.Read == TokenStream[InputPointer].token_type)
                {

                    state.Children.Add(readS());
                }
                else if (Token_Class.Repeat == TokenStream[InputPointer].token_type)
                {

                    state.Children.Add(repeatS());
                }
                else if (Token_Class.Write == TokenStream[InputPointer].token_type)
                {

                    state.Children.Add(writeS());
                }
                else if (Token_Class.If == TokenStream[InputPointer].token_type)
                {
                    state.Children.Add(If_statement()); //////call ifstate here with all status

                }
                else if (Token_Class.Identifier == TokenStream[InputPointer].token_type && Token_Class.Assign == TokenStream[InputPointer + 1].token_type)
                {

                    state.Children.Add(Assignment_state());/////call assignment statment

                }


                else if (Token_Class.DataType_Int == TokenStream[InputPointer].token_type || Token_Class.DataType_String == TokenStream[InputPointer].token_type
                   || Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {
                    state.Children.Add(Declaration_state());////call decleration statment

                }
                else if (Token_Class.Identifier == TokenStream[InputPointer].token_type)

                {
                    if (Token_Class.LParanthesis == TokenStream[InputPointer + 1].token_type)
                        state.Children.Add(Function_call());

                }
                else
                {
                    return null;
                }
            }
            else
                return null;
            return state;

        }



        Node States()
        {


            Node states = new Node("states");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Read == TokenStream[InputPointer].token_type ||
                    Token_Class.Write == TokenStream[InputPointer].token_type ||
                    Token_Class.Comment == TokenStream[InputPointer].token_type ||
                    Token_Class.Repeat == TokenStream[InputPointer].token_type ||
                    Token_Class.Identifier == TokenStream[InputPointer].token_type ||
                    Token_Class.If == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Int == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_String == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Float == TokenStream[InputPointer].token_type)
                {
                    states.Children.Add(statement());
                    states.Children.Add(States());
                }
                return states;
            }
            return null;

        }

        Node Statments()

        {
            Node statments = new Node("statments");

            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Read == TokenStream[InputPointer].token_type ||
                    Token_Class.Write == TokenStream[InputPointer].token_type ||
                    Token_Class.Identifier == TokenStream[InputPointer].token_type ||
                    Token_Class.If == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Int == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_String == TokenStream[InputPointer].token_type ||
                    Token_Class.Comment == TokenStream[InputPointer].token_type ||
                    Token_Class.Repeat == TokenStream[InputPointer].token_type ||
                    Token_Class.DataType_Float == TokenStream[InputPointer].token_type)


                {
                    statments.Children.Add(statement());
                    statments.Children.Add(States());
                }
            }

            return statments;
        }

        Node func_body()
        {   //{statments returnstatment}

            Node func_body = new Node("func_body");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LBraces == TokenStream[InputPointer].token_type)
                {

                    func_body.Children.Add(match(Token_Class.LBraces));
                    func_body.Children.Add(Statments());
                    func_body.Children.Add(returnS());
                    func_body.Children.Add(match(Token_Class.RBraces));

                }
                else
                    return null;
            }
            else
                return null;
            return func_body;
        }
        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }


    }
}