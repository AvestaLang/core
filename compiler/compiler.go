package compiler

import (
	"fmt"
	"html"
	"strings"

	"github.com/avestalang/core/lib"
)

//
// =======================
// AST
// =======================
//

type Node struct {
	Tag        string
	Attributes map[string]string
	Children   []*Node
	Text       string
}

//
// =======================
// TOKENS
// =======================
//

type TokenType int

const (
	TokenTag TokenType = iota
	TokenEnd
	TokenAttr
)

type Token struct {
	Type  TokenType
	Key   string
	Value string
}

//
// =======================
// TRANSLATION
// =======================
//

var tagMap = map[string]string{
	"جعبه": "div",
	"دکمه": "button",
}

//
// =======================
// LEXER
// =======================
//

func lex(input string) ([]Token, error) {
	lines := strings.Split(input, "\n")
	var tokens []Token

	for i, line := range lines {

		t := strings.TrimSpace(line)
		if t == "" {
			continue
		}

		switch {

		case strings.HasSuffix(t, ":"):
			name := strings.TrimSuffix(t, ":")
			tokens = append(tokens, Token{
				Type:  TokenTag,
				Key:   name,
				Value: "",
			})

		case t == "پایان":
			tokens = append(tokens, Token{
				Type: TokenEnd,
			})

		case strings.Contains(t, "="):
			parts := strings.SplitN(t, "=", 2)
			if len(parts) != 2 {
				return nil, fmt.Errorf("invalid attribute at line %d", i+1)
			}

			val := strings.Trim(parts[1], "«»")

			tokens = append(tokens, Token{
				Type:  TokenAttr,
				Key:   parts[0],
				Value: val,
			})
		}
	}

	return tokens, nil
}

//
// =======================
// PARSER → AST
// =======================
//

func parse(tokens []Token) (*Node, error) {

	root := &Node{
		Tag:        "root",
		Attributes: map[string]string{},
	}

	stack := []*Node{root}

	for _, tok := range tokens {

		switch tok.Type {

		case TokenTag:

			htmlTag, ok := tagMap[tok.Key]
			if !ok {
				return nil, fmt.Errorf("unknown tag %s", tok.Key)
			}

			node := &Node{
				Tag:        htmlTag,
				Attributes: map[string]string{},
			}

			parent := stack[len(stack)-1]
			parent.Children = append(parent.Children, node)

			stack = append(stack, node)

		case TokenAttr:

			parent := stack[len(stack)-1]

			if tok.Key == "محتوا" {
				parent.Text = tok.Value
			} else {
				parent.Attributes[tok.Key] = tok.Value
			}

		case TokenEnd:

			if len(stack) == 1 {
				return nil, fmt.Errorf("unexpected پایان")
			}

			stack = stack[:len(stack)-1]
		}
	}

	if len(stack) != 1 {
		return nil, fmt.Errorf("unclosed tags detected")
	}

	return root, nil
}

//
// =======================
// RENDERER
// =======================
//

func render(n *Node, b *strings.Builder) {

	if n.Tag != "root" {

		b.WriteString("<" + n.Tag)

		for k, v := range n.Attributes {
			b.WriteString(fmt.Sprintf(` %s="%s"`, k, html.EscapeString(v)))
		}

		b.WriteString(">")
	}

	if n.Text != "" {
		b.WriteString(html.EscapeString(n.Text))
	}

	for _, c := range n.Children {
		render(c, b)
	}

	if n.Tag != "root" {
		b.WriteString("</" + n.Tag + ">")
	}
}

//
// =======================
// RENDERER
// =======================
//

var styles = []string{
	"button.css",
}

func Styles() string {
	all := ""

	for _, file := range styles {
		content, _ := lib.Reader("./styles/" + file)

		all += content + "\n"
	}

	return all
}

//
// =======================
// PUBLIC COMPILER
// =======================
//

func Compile(input string) (string, error) {

	tokens, err := lex(input)
	if err != nil {
		return "", err
	}

	ast, err := parse(tokens)
	if err != nil {
		return "", err
	}

	var b strings.Builder

	b.WriteString(`<!DOCTYPE html><html dir="rtl" lang="fa"><head><meta charset="utf-8"><style>`)

	styles := Styles()
	b.WriteString(styles)

	b.WriteString(`</style></head><body>`)

	render(ast, &b)

	b.WriteString("</body></html>")

	return b.String(), nil
}
