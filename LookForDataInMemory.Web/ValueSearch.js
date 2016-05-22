function test()
{
	var a =
	{
		Id: 1,
		Name: "Peter",
		Age: 25
	};

	var b =
	{
		Id: 1,
		Name: "Peter",
		// это свойство нужно, чтобы индекс следующего был уже другим,
		// чтобы протестировать лучше алгоритм
		Something: 10,
		Age: 26,
	};

	findAndAlertObjectDifference(a, b);
}

/// пока что находит только первую разницу среди свойств, присутствующих
/// в первом объекте
function findAndAlertObjectDifference(a, b)
{
	var state1 = getStateDictionary(a);
	var state2 = getStateDictionary(b);

	findAndAlertStateDifference(state1, state2);
}

function findAndAlertStateDifference(state1, state2)
{
	var res = getStateDifference(state1, state2);
	var text = "";

	if (res.length > 0)
	{
		if (res.length > 5)
			text += "Первые 5 результатов:\n";
		else
			text += "Результат(ы):\n"

		for (var i = 0; i < res.length && i < 5; i++)
		{
			var r = res[i];
			var res1 = r.State1Index;
			var res2 = r.State2Index;

			text += "\nСвойство: \"" + state1[res1].key + "\":\n" +
					" значение в первом объекте: \"" + state1[res1].value + "\",\n" +
					" значение во втором: \"" + state2[res2].value +
					"\"\n";

		}

		alert(text);
	}
	else
	{
		alert("Разницы в свойствах нет");
	}
}

/// пока что находит только первую разницу среди свойств, присутствующих
/// в первом списке
function getStateDifference(state1, state2)
{
	var found = false;
	var results = new Array();

	for (var i = 0; i < state1.length; i++)
	{
		for (var j = 0; j < state2.length; j++)
		{
			if (state1[i].key == state2[j].key)
			{
				if (state1[i].value != state2[j].value)
				{
					res1 = i;
					res2 = j;
					results.push(
						{
							State1Index: i,
							State2Index: j
						});
					found = true;
				}

				break;
			}
		}
	}

	return results;
}

function getStateDictionary(target)
{
	var dict = [];

	for (p in target)
	{
		dict.push({
			key: p != null ? p.toString() : "",
			value: target[p] != null ? target[p].toString() : ""
		});
	}

	return dict;
}

