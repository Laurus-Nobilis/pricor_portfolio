<?php
/**
 * @var \App\View\AppView $this
 * @var \App\Model\Entity\Userdata $userdata
 */
?>
<nav class="large-3 medium-4 columns" id="actions-sidebar">
    <ul class="side-nav">
        <li class="heading"><?= __('Actions') ?></li>
        <li><?= $this->Html->link(__('Edit Userdata'), ['action' => 'edit', $userdata->id]) ?> </li>
        <li><?= $this->Form->postLink(__('Delete Userdata'), ['action' => 'delete', $userdata->id], ['confirm' => __('Are you sure you want to delete # {0}?', $userdata->id)]) ?> </li>
        <li><?= $this->Html->link(__('List Userdata'), ['action' => 'index']) ?> </li>
        <li><?= $this->Html->link(__('New Userdata'), ['action' => 'add']) ?> </li>
    </ul>
</nav>
<div class="userdata view large-9 medium-8 columns content">
    <h3><?= h($userdata->name) ?></h3>
    <table class="vertical-table">
        <tr>
            <th scope="row"><?= __('Name') ?></th>
            <td><?= h($userdata->name) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Id') ?></th>
            <td><?= $this->Number->format($userdata->id) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Rank') ?></th>
            <td><?= $this->Number->format($userdata->rank) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Exp') ?></th>
            <td><?= $this->Number->format($userdata->exp) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Lastlogin') ?></th>
            <td><?= h($userdata->lastlogin) ?></td>
        </tr>
        <tr>
            <th scope="row"><?= __('Created') ?></th>
            <td><?= h($userdata->created) ?></td>
        </tr>
    </table>
</div>
