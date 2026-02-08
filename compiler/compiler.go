package compiler

import (
	"errors"
	"strings"
)

func Compile(input string) string {
	result := strings.Builder{}

	result.WriteString("<!DOCTYPE html>")
	result.WriteString("<html lang=\"fa\"><head>")
	result.WriteString("<meta charset=\"utf-8\">")
	result.WriteString("</head><body>")

	lines := strings.Split(input, "\n")
	stack := []string{}

	for _, line := range lines {
		trimmed := strings.TrimSpace(line)

		if trimmed == "" {
			continue
		}

		if strings.HasSuffix(trimmed, ":") {
			translated, err := translate(strings.Trim(trimmed, ":"))

			if translated == "" {
				continue
			}

			if err != nil {
				panic(err)
			}

			result.WriteString("<" + translated + ">")
			stack = append(stack, translated)
		}

		if strings.Contains(trimmed, "=") && strings.Contains(trimmed, "«") && strings.Contains(trimmed, "»") {
			println("Property Finded")
		}

		if trimmed == "پایان" {
			result.WriteString("</" + stack[len(stack)-1] + ">")
			stack = stack[:len(stack)-1]
		}
	}

	result.WriteString("</body></html>")
	return result.String()
}

func translate(tag string) (string, error) {
	tags := map[string]string{
		"دکمه": "button",
		"جعبه": "div",
	}

	if hTag, exists := tags[tag]; exists {
		return hTag, nil
	}

	return "", errors.New("Cant find translate of => " + tag)
}
