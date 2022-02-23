using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Class
{
    Semicolon, Comma, LParanthesis, RParanthesis, Equal, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Identifier, Number, Undefiend, DataType_Int, DataType_Float,
    DataType_String, Read, Write, Repeat, Endl_T, Return,
    Then, If, Else, Elseif, Until, Colon, LBraces, RBraces, MainFunction,
    Comment, singleQouts, doubleQouts, Assign, String, Or, And, end
}

namespace Tiny_Compiler
{

    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> other = new Dictionary<string, Token_Class>();
        public Scanner()
        {

            ReservedWords.Add("int", Token_Class.DataType_Int);
            ReservedWords.Add("main", Token_Class.MainFunction);
            ReservedWords.Add("float", Token_Class.DataType_Float);
            ReservedWords.Add("string", Token_Class.DataType_String);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("endl", Token_Class.Endl_T);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("comment", Token_Class.Comment);
            ReservedWords.Add("String", Token_Class.String);
            ReservedWords.Add("iden", Token_Class.Identifier);
            ReservedWords.Add("number", Token_Class.Number);
            ReservedWords.Add("end", Token_Class.end);

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LBraces);
            Operators.Add("}", Token_Class.RBraces);
            Operators.Add("=", Token_Class.Equal);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("\'", Token_Class.singleQouts);
            Operators.Add(":=", Token_Class.Assign);
            Operators.Add("||", Token_Class.Or);
            Operators.Add("&&", Token_Class.And);
            other.Add("Un", Token_Class.Undefiend);


        }
        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                Token Tok = new Token();
                Tok.lex = CurrentLexeme;
                string checkStr = "";
                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                {
                    continue;
                }
                // for := token
                else if (CurrentChar == ':' && SourceCode[j + 1] == '=')
                {
                    FindTokenClass(":=");
                    j = j + 2;
                    i = j - 1;
                }
                // for <> token
                else if (CurrentChar == '<' && SourceCode[j + 1] == '>')//<>
                {
                    FindTokenClass("<>");
                    j = j + 2;
                    i = j - 1;

                }
                else if (CurrentChar == '&' && SourceCode[j + 1] == '&')//<>
                {
                    FindTokenClass("&&");
                    j = j + 2;
                    i = j - 1;
                }
                else if (CurrentChar == '|' && SourceCode[j + 1] == '|')//<>
                {
                    FindTokenClass("||");
                    j = j + 2;
                    i = j - 1;

                }
                // for string
                else if (CurrentChar == '\"')//<>
                {
                    checkStr += CurrentChar;
                    j++;
                    if (j > SourceCode.Length - 1)
                        break;
                    CurrentChar = SourceCode[j];
                    while (true)
                    {
                        if (CurrentChar == '\"')
                        {

                            checkStr += CurrentChar;

                            j++;
                            if (j > SourceCode.Length - 1)
                                break;
                            break;
                        }
                        else
                        {
                            checkStr += CurrentChar;
                            j++;
                            if (j > SourceCode.Length - 1)
                                break;
                            CurrentChar = SourceCode[j];
                        }

                    }
                    CurrentLexeme = CurrentChar.ToString();
                    FindTokenClass(checkStr);
                    i = j - 1;


                }
                // for comment
                else if (Operators.ContainsKey(CurrentChar.ToString()))
                {
                    if (CurrentChar == '/' && SourceCode[j + 1] != '*')//<>
                    {
                        FindTokenClass("/");
                        j = j + 1;
                        i = j - 1;
                    }
                    else
                    {
                        string commentStr = "";
                        commentStr += CurrentChar;
                        while (j < SourceCode.Length)
                        {
                            if (CurrentChar == '/')
                            {
                                if (j < SourceCode.Length - 1)
                                    j++;
                                CurrentChar = SourceCode[j];
                                commentStr += CurrentChar;

                                if (CurrentChar == '*')
                                {

                                    while (j < SourceCode.Length - 1)
                                    {
                                        j++;
                                        CurrentChar = SourceCode[j];
                                        commentStr += CurrentChar;
                                        if (CurrentChar == '*')
                                        {
                                            if (j < SourceCode.Length)
                                                j++;
                                            CurrentChar = SourceCode[j];
                                            commentStr += CurrentChar;
                                            if (CurrentChar == '/')
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            FindTokenClass(commentStr);
                            i = j;
                            break;
                        }
                    }
                }


                else if (char.IsLetter(CurrentChar))
                {

                    while (true)
                    {
                        if (CurrentChar == ' ' || CurrentChar == ':' || CurrentChar == '\r' || CurrentChar == '\n' || Operators.ContainsKey(CurrentChar.ToString()))
                            break;

                        checkStr += CurrentChar;
                        j++;
                        if (j >= SourceCode.Length)
                            break;
                        CurrentChar = SourceCode[j];

                    }

                    CurrentLexeme = checkStr;
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }


                else if (char.IsDigit(CurrentChar))
                {
                    CurrentLexeme = "";
                    while (j < SourceCode.Length - 1)
                    {

                        CurrentLexeme += CurrentChar;
                        j++;
                        CurrentChar = SourceCode[j];
                        if (j >= SourceCode.Length)
                            break;

                        if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                            break;
                        if (Operators.ContainsKey(CurrentChar.ToString()))
                            break;


                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                }

                else
                {
                    FindTokenClass(CurrentLexeme);
                }



            }
            Tiny_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is comment?
            if (isComment(Lex))
            {
                Tok.token_type = ReservedWords["comment"];
                Tokens.Add(Tok);
            }
            else if (isString(Lex))
            {
                Tok.token_type = ReservedWords["String"];
                Tokens.Add(Tok);
            }
            //Is it a reserved word?
            else if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = ReservedWords["iden"];
                Tokens.Add(Tok);
            }
            //Is it a Constant?
            else if (isNumber(Lex))
            {
                Tok.token_type = ReservedWords["number"];
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add("Unrecognized token \t" + Lex);
            }
        }

        bool isIdentifier(string lex)
        {
            bool isValid = false;
            // Check if the lex is an identifier or not.
            var rx = new Regex(@"^[a-zA-z_][a-zA-z0-9_]*$", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
            {
                isValid = true;
            }
            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = false;
            // Check if the lex is a constant (Number) or not.
            var rx = new Regex(@"^[0-9]+(.[0-9]+)?$", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
            {
                isValid = true;
            }
            return isValid;
        }
        bool isComment(string lex)
        {
            bool isValid = false;
            // Check if the lex is a comment or not.
            ///\[\s\S]?\*/
            var rx = new Regex(@"^/\*.*\*/", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
            {
                isValid = true;
            }
            return isValid;
        }
        bool isString(string lex)
        {
            bool isValid = false;
            var rx = new Regex("^\".*\"$", RegexOptions.Compiled);
            if (rx.IsMatch(lex))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}